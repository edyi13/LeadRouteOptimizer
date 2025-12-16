using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadRouteOptimizer.Application.Plans.Commands
{
    public sealed record CreatePlanResult(
        Guid PlanId,
        int Stops,
        decimal TotalDistanceKm
    );
}
