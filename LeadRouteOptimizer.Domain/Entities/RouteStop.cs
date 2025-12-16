using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadRouteOptimizer.Domain.Entities
{
    public sealed class RouteStop
    {
        public Guid RoutePlanId { get; set; }
        public RoutePlan RoutePlan { get; set; } = default!;

        public int Sequence { get; set; }

        public Guid LeadId { get; set; }
        public Lead Lead { get; set; } = default!;

        public decimal LegDistanceKm { get; set; }

        public DateTime CreatedAtUtc { get; set; }
    }
}
