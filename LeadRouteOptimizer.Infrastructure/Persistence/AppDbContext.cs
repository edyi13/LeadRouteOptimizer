using LeadRouteOptimizer.Domain.Entities;
using LeadRouteOptimizer.Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadRouteOptimizer.Infrastructure.Persistence
{
    public sealed class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<UploadBatch> UploadBatches => Set<UploadBatch>();
        public DbSet<Lead> Leads => Set<Lead>();
        public DbSet<RoutePlan> RoutePlans => Set<RoutePlan>();
        public DbSet<RouteStop> RouteStops => Set<RouteStop>();
        public DbSet<RoutePlanUpload> RoutePlanUploads => Set<RoutePlanUpload>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("app");

            modelBuilder.ApplyConfiguration(new UploadBatchConfig());
            modelBuilder.ApplyConfiguration(new LeadConfig());
            modelBuilder.ApplyConfiguration(new RoutePlanConfig());
            modelBuilder.ApplyConfiguration(new RouteStopConfig());
            modelBuilder.ApplyConfiguration(new RoutePlanUploadConfig());
        }
    }
}
