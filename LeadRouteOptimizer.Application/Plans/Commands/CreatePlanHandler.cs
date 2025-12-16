using LeadRouteOptimizer.Application.Interfaces;
using LeadRouteOptimizer.Domain.Entities;
using LeadRouteOptimizer.Domain.Services;

namespace LeadRouteOptimizer.Application.Plans.Commands
{
    public class CreatePlanHandler(
        ILeadRepository leadRepo,
        IRoutePlanRepository planRepo,
        IUnitOfWork uow)
    {
        public async Task<CreatePlanResult> HandleAsync(CreatePlanCommand cmd, CancellationToken ct)
        {
            if (cmd.HomeLatitude < -90m || cmd.HomeLatitude > 90m)
                throw new ArgumentException("HomeLatitude must be between -90 and 90.");
            if (cmd.HomeLongitude < -180m || cmd.HomeLongitude > 180m)
                throw new ArgumentException("HomeLongitude must be between -180 and 180.");
            if (cmd.UploadBatchIds is null || cmd.UploadBatchIds.Count == 0)
                throw new ArgumentException("At least one UploadBatchId is required.");

            var leads = await leadRepo.GetValidLeadsByUploadBatchIdsAsync(cmd.UploadBatchIds, ct);

            // dedupe by normalized key
            var distinct = leads
                .GroupBy(l => l.NormalizedKey)
                .Select(g => g.First())
                .ToList();

            if (distinct.Count == 0)
                throw new ArgumentException("No valid leads found for the provided uploads.");

            var plan = new RoutePlan
            {
                HomeLatitude = cmd.HomeLatitude,
                HomeLongitude = cmd.HomeLongitude,
                TotalDistanceKm = 0m,
                CreatedAtUtc = DateTime.UtcNow
            };

            // link uploads to plan
            foreach (var uploadId in cmd.UploadBatchIds.Distinct())
            {
                plan.RoutePlanUploads.Add(new RoutePlanUpload
                {
                    UploadBatchId = uploadId,
                    LinkedAtUtc = DateTime.UtcNow
                });
            }

            // build route using nearest neighbor heuristic
            var remaining = new HashSet<Guid>(distinct.Select(l => l.Id));
            var lookup = distinct.ToDictionary(l => l.Id);

            decimal currLat = cmd.HomeLatitude;
            decimal currLon = cmd.HomeLongitude;

            var seq = 1;
            decimal total = 0m;

            while (remaining.Count > 0)
            {
                Guid bestId = default;
                decimal bestDist = decimal.MaxValue;

                foreach (var id in remaining)
                {
                    var lead = lookup[id];
                    var d = Haversine.DistanceKm(currLat, currLon, lead.Latitude, lead.Longitude);

                    // tie-breaker on Guid to ensure deterministic results
                    if (d < bestDist || (d == bestDist && id.CompareTo(bestId) < 0))
                    {
                        bestDist = d;
                        bestId = id;
                    }
                }

                remaining.Remove(bestId);
                var next = lookup[bestId];

                total += bestDist;

                plan.Stops.Add(new RouteStop
                {
                    Sequence = seq++,
                    LeadId = next.Id,
                    LegDistanceKm = bestDist,
                    CreatedAtUtc = DateTime.UtcNow
                });

                currLat = next.Latitude;
                currLon = next.Longitude;
            }

            plan.TotalDistanceKm = total;

            await planRepo.AddAsync(plan, ct);
            await uow.SaveChangesAsync(ct);

            return new CreatePlanResult(plan.Id, plan.Stops.Count, plan.TotalDistanceKm);
        }
    }
}
