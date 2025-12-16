using LeadRouteOptimizer.Domain.Entities;

namespace LeadRouteOptimizer.Application.Interfaces
{
    public interface IUploadBatchRepository
    {
        Task<UploadBatch?> GetByIdAsync(Guid id, CancellationToken ct);
        Task AddAsync(UploadBatch batch, CancellationToken ct);
    }
}
