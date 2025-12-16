using LeadRouteOptimizer.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadRouteOptimizer.Infrastructure.Persistence.Configurations
{
    internal sealed class RoutePlanUploadConfig : IEntityTypeConfiguration<RoutePlanUpload>
    {
        public void Configure(EntityTypeBuilder<RoutePlanUpload> b)
        {
            b.ToTable("RoutePlanUpload");

            b.HasKey(x => new { x.RoutePlanId, x.UploadBatchId });

            b.Property(x => x.LinkedAtUtc)
                .IsRequired();

            b.HasOne(x => x.RoutePlan)
                .WithMany(x => x.RoutePlanUploads)
                .HasForeignKey(x => x.RoutePlanId)
                .OnDelete(DeleteBehavior.Cascade);

            b.HasOne(x => x.UploadBatch)
                .WithMany()
                .HasForeignKey(x => x.UploadBatchId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
