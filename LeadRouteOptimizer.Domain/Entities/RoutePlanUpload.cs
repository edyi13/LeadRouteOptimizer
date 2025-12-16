using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadRouteOptimizer.Domain.Entities
{
    public sealed class RoutePlanUpload
    {
        public Guid RoutePlanId { get; set; }
        public RoutePlan RoutePlan { get; set; } = default!;

        public Guid UploadBatchId { get; set; }
        public UploadBatch UploadBatch { get; set; } = default!;

        public DateTime LinkedAtUtc { get; set; }
    }
}
