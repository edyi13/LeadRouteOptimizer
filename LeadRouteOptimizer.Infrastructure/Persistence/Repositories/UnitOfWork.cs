using LeadRouteOptimizer.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadRouteOptimizer.Infrastructure.Persistence.Repositories
{
    internal sealed class UnitOfWork(AppDbContext db) : IUnitOfWork
    {
        public Task<int> SaveChangesAsync(CancellationToken ct) => db.SaveChangesAsync(ct);
    }
}
