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
    internal sealed class LeadRepository(AppDbContext db) : ILeadRepository
    {
        public Task AddRangeAsync(IEnumerable<Lead> leads, CancellationToken ct) =>
            db.Leads.AddRangeAsync(leads, ct);

        public async Task<IReadOnlyList<Lead>> GetValidLeadsByUploadBatchIdsAsync(
            IReadOnlyCollection<Guid> uploadBatchIds,
            CancellationToken ct)
        {
            return await db.Leads
                .AsNoTracking()
                .Where(l => l.Status == "Valid" && uploadBatchIds.Contains(l.UploadBatchId))
                .ToListAsync(ct);
        }
    }
}
