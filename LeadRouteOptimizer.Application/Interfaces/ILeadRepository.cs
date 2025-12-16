using LeadRouteOptimizer.Domain.Entities;

namespace LeadRouteOptimizer.Application.Interfaces
{
    public interface ILeadRepository
    {
        Task AddRangeAsync(IEnumerable<Lead> leads, CancellationToken ct);

        Task<IReadOnlyList<Lead>> GetValidLeadsByUploadBatchIdsAsync(
            IReadOnlyCollection<Guid> uploadBatchIds,
            CancellationToken ct);
    }
}
