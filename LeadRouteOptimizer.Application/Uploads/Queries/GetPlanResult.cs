namespace LeadRouteOptimizer.Application.Uploads.Queries
{
    public record GetPlanResult(
        Guid PlanId,
        decimal HomeLatitude,
        decimal HomeLongitude,
        decimal TotalDistanceKm,
        IReadOnlyList<GetPlanStop> Stops
    );

    public record GetPlanStop(
        int Sequence,
        decimal LegDistanceKm,
        Guid LeadId,
        string LeadName,
        string? Street,
        string? City,
        string? State,
        string? Zip,
        decimal Latitude,
        decimal Longitude
    );
}
