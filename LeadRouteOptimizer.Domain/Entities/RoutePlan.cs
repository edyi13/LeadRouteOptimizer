using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadRouteOptimizer.Domain.Entities
{
    public sealed class RoutePlan
    {
        public Guid Id { get; set; }

        public decimal HomeLatitude { get; set; }
        public decimal HomeLongitude { get; set; }

        public decimal TotalDistanceKm { get; set; }

        public DateTime CreatedAtUtc { get; set; }

        public List<RouteStop> Stops { get; set; } = new();
        public List<RoutePlanUpload> RoutePlanUploads { get; set; } = new();
    }
}
