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
    internal sealed class UploadBatchRepository(AppDbContext db) : IUploadBatchRepository
    {
        public Task<UploadBatch?> GetByIdAsync(Guid id, CancellationToken ct) =>
            db.UploadBatches.FirstOrDefaultAsync(x => x.Id == id, ct);

        public Task AddAsync(UploadBatch batch, CancellationToken ct) =>
            db.UploadBatches.AddAsync(batch, ct).AsTask();
    }
}
