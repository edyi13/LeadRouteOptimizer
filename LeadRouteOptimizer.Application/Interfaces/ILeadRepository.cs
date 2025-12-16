using LeadRouteOptimizer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
