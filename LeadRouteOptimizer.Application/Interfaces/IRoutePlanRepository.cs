using LeadRouteOptimizer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadRouteOptimizer.Application.Interfaces
{
    public interface IRoutePlanRepository
    {
        Task AddAsync(RoutePlan plan, CancellationToken ct);

        Task<RoutePlan?> GetPlanWithStopsAsync(Guid planId, CancellationToken ct);
    }
}
