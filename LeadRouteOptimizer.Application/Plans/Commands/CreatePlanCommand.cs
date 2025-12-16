namespace LeadRouteOptimizer.Application.Plans.Commands
{
    public record CreatePlanCommand(
        decimal HomeLatitude,
        decimal HomeLongitude,
        IReadOnlyList<Guid> UploadBatchIds
    );
}
