namespace LeadRouteOptimizer.Application.Plans.Commands
{
    public record CreatePlanResult(
        Guid PlanId,
        int Stops,
        decimal TotalDistanceKm
    );
}
