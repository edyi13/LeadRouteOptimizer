namespace LeadRouteOptimizer.Api.Contracts
{
    public sealed class CreatePlanRequest
    {
        public decimal HomeLatitude { get; set; }
        public decimal HomeLongitude { get; set; }
        public List<Guid> UploadBatchIds { get; set; } = new();
    }
}
