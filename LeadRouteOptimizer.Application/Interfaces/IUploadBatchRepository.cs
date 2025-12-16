using LeadRouteOptimizer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadRouteOptimizer.Application.Interfaces
{
    public interface IUploadBatchRepository
    {
        Task<UploadBatch?> GetByIdAsync(Guid id, CancellationToken ct);
        Task AddAsync(UploadBatch batch, CancellationToken ct);
    }
}
