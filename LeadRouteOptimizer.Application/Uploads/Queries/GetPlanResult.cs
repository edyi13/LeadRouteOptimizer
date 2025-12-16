using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadRouteOptimizer.Application.Uploads.Queries
{
    public sealed record GetPlanResult(
        Guid PlanId,
        decimal HomeLatitude,
        decimal HomeLongitude,
        decimal TotalDistanceKm,
        IReadOnlyList<GetPlanStop> Stops
    );

    public sealed record GetPlanStop(
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
