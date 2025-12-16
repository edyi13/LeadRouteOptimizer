using LeadRouteOptimizer.Application.Interfaces;
using LeadRouteOptimizer.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadRouteOptimizer.Infrastructure.Persistence.Repositories
{
    internal sealed class RoutePlanRepository(AppDbContext db) : IRoutePlanRepository
    {
        public Task AddAsync(RoutePlan plan, CancellationToken ct) =>
            db.RoutePlans.AddAsync(plan, ct).AsTask();

        public Task<RoutePlan?> GetPlanWithStopsAsync(Guid planId, CancellationToken ct) =>
            db.RoutePlans
              .AsNoTracking()
              .Include(p => p.Stops)
                .ThenInclude(s => s.Lead)
              .FirstOrDefaultAsync(p => p.Id == planId, ct);
    }
}
