namespace LeadRouteOptimizer.Domain.Entities
{
    public class RoutePlanUpload
    {
        public Guid RoutePlanId { get; set; }
        public RoutePlan RoutePlan { get; set; } = default!;

        public Guid UploadBatchId { get; set; }
        public UploadBatch UploadBatch { get; set; } = default!;

        public DateTime LinkedAtUtc { get; set; }
    }
}
