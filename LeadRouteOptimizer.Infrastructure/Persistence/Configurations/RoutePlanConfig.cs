using LeadRouteOptimizer.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LeadRouteOptimizer.Infrastructure.Persistence.Configurations
{
    internal class RoutePlanConfig : IEntityTypeConfiguration<RoutePlan>
    {
        public void Configure(EntityTypeBuilder<RoutePlan> b)
        {
            b.ToTable("RoutePlan");

            b.HasKey(x => x.Id);

            b.Property(x => x.HomeLatitude)
                .HasColumnType("decimal(9,6)")
                .IsRequired();

            b.Property(x => x.HomeLongitude)
                .HasColumnType("decimal(9,6)")
                .IsRequired();

            b.Property(x => x.TotalDistanceKm)
                .HasColumnType("decimal(12,3)")
                .IsRequired();

            b.Property(x => x.CreatedAtUtc)
                .IsRequired();

            b.HasMany(x => x.Stops)
                .WithOne(x => x.RoutePlan)
                .HasForeignKey(x => x.RoutePlanId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
