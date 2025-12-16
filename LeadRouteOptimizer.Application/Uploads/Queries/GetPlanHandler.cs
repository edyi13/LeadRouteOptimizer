using LeadRouteOptimizer.Application.Interfaces;

namespace LeadRouteOptimizer.Application.Uploads.Queries
{
    public class GetPlanHandler(IRoutePlanRepository planRepo)
    {
        public async Task<GetPlanResult?> HandleAsync(GetPlanQuery q, CancellationToken ct)
        {
            var plan = await planRepo.GetPlanWithStopsAsync(q.PlanId, ct);
            if (plan is null) return null;

            var stops = plan.Stops
                .OrderBy(s => s.Sequence)
                .Select(s => new GetPlanStop(
                    s.Sequence,
                    s.LegDistanceKm,
                    s.LeadId,
                    s.Lead.LeadName,
                    s.Lead.Street,
                    s.Lead.City,
                    s.Lead.State,
                    s.Lead.Zip,
                    s.Lead.Latitude,
                    s.Lead.Longitude
                ))
                .ToList();

            return new GetPlanResult(
                plan.Id,
                plan.HomeLatitude,
                plan.HomeLongitude,
                plan.TotalDistanceKm,
                stops
            );
        }
    }
}
