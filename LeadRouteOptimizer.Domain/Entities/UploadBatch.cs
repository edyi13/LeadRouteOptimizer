namespace LeadRouteOptimizer.Domain.Entities
{
    public class UploadBatch
    {
        public Guid Id { get; set; }

        // manager/personal
        public string SourceType { get; set; } = default!;

        public string? OriginalFileName { get; set; }

        public DateTime CreatedAtUtc { get; set; }

        public string? CreatedBy { get; set; }

        public List<Lead> Leads { get; set; } = new();
    }
}
