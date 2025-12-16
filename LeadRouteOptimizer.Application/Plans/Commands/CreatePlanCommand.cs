using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadRouteOptimizer.Application.Plans.Commands
{
    public sealed record CreatePlanCommand(
        decimal HomeLatitude,
        decimal HomeLongitude,
        IReadOnlyList<Guid> UploadBatchIds
    );
}
