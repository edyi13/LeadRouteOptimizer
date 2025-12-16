using LeadRouteOptimizer.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LeadRouteOptimizer.Infrastructure.Persistence.Configurations
{
    internal class RouteStopConfig : IEntityTypeConfiguration<RouteStop>
    {
        public void Configure(EntityTypeBuilder<RouteStop> b)
        {
            b.ToTable("RouteStop");

            b.HasKey(x => new { x.RoutePlanId, x.Sequence });

            b.Property(x => x.Sequence).IsRequired();

            b.Property(x => x.LegDistanceKm)
                .HasColumnType("decimal(12,3)")
                .IsRequired();

            b.Property(x => x.CreatedAtUtc)
                .IsRequired();

            b.HasOne(x => x.Lead)
                .WithMany()
                .HasForeignKey(x => x.LeadId)
                .OnDelete(DeleteBehavior.Restrict);

            b.HasIndex(x => x.LeadId);
        }
    }
}
