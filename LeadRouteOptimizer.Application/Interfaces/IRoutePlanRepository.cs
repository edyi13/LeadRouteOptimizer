using LeadRouteOptimizer.Domain.Entities;

namespace LeadRouteOptimizer.Application.Interfaces
{
    public interface IRoutePlanRepository
    {
        Task AddAsync(RoutePlan plan, CancellationToken ct);

        Task<RoutePlan?> GetPlanWithStopsAsync(Guid planId, CancellationToken ct);
    }
}
