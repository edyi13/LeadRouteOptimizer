namespace LeadRouteOptimizer.Domain.Entities
{
    public class Lead
    {
        public Guid Id { get; set; }

        public Guid UploadBatchId { get; set; }
        public UploadBatch UploadBatch { get; set; } = default!;

        public string LeadName { get; set; } = default!;

        public string? Street { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Zip { get; set; }

        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }

        // computed in db
        public string NormalizedKey { get; private set; } = default!;

        // valid/invalid
        public string Status { get; set; } = default!;
        public string? ErrorMessage { get; set; }

        public string? RawRowJson { get; set; }

        public DateTime CreatedAtUtc { get; set; }
    }
}
