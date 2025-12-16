using LeadRouteOptimizer.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LeadRouteOptimizer.Infrastructure.Persistence.Configurations
{
    internal class UploadBatchConfig : IEntityTypeConfiguration<UploadBatch>
    {
        public void Configure(EntityTypeBuilder<UploadBatch> b)
        {
            b.ToTable("UploadBatch");

            b.HasKey(x => x.Id);

            b.Property(x => x.SourceType)
                .HasMaxLength(20)
                .IsRequired();

            b.Property(x => x.OriginalFileName)
                .HasMaxLength(260);

            b.Property(x => x.CreatedBy)
                .HasMaxLength(100);

            b.Property(x => x.CreatedAtUtc)
                .IsRequired();

            b.HasMany(x => x.Leads)
                .WithOne(x => x.UploadBatch)
                .HasForeignKey(x => x.UploadBatchId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
