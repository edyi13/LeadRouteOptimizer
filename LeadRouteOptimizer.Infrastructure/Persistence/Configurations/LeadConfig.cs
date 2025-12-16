using LeadRouteOptimizer.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LeadRouteOptimizer.Infrastructure.Persistence.Configurations
{
    internal class LeadConfig : IEntityTypeConfiguration<Lead>
    {
        public void Configure(EntityTypeBuilder<Lead> b)
        {
            b.ToTable("Lead");

            b.HasKey(x => x.Id);

            b.Property(x => x.LeadName)
                .HasMaxLength(200)
                .IsRequired();

            b.Property(x => x.Street).HasMaxLength(200);
            b.Property(x => x.City).HasMaxLength(100);
            b.Property(x => x.State).HasMaxLength(50);
            b.Property(x => x.Zip).HasMaxLength(20);

            b.Property(x => x.Latitude)
                .HasColumnType("decimal(9,6)")
                .IsRequired();

            b.Property(x => x.Longitude)
                .HasColumnType("decimal(9,6)")
                .IsRequired();

            b.Property(x => x.Status)
                .HasMaxLength(20)
                .IsRequired();

            b.Property(x => x.ErrorMessage)
                .HasMaxLength(1000);

            b.Property(x => x.RawRowJson)
                .HasColumnType("nvarchar(max)");

            b.Property(x => x.CreatedAtUtc)
                .IsRequired();

            // Computed column in SQL. EF must treat it as computed.
            b.Property(x => x.NormalizedKey)
                .HasColumnType("nvarchar(400)") // enough for name + coords
                .HasComputedColumnSql(
                    "LOWER(LTRIM(RTRIM([LeadName]))) + N'|' + CONVERT(NVARCHAR(50), ROUND([Latitude], 5)) + N'|' + CONVERT(NVARCHAR(50), ROUND([Longitude], 5))",
                    stored: true);

            b.HasIndex(x => x.UploadBatchId);
            b.HasIndex(x => x.NormalizedKey);
        }
    }
}
