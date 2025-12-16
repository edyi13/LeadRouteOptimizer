using LeadRouteOptimizer.Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadRouteOptimizer.Tests.Integration
{
    public sealed class TestAppFactory : WebApplicationFactory<Program>
    {
        private readonly string _connString;

        public TestAppFactory()
        {
            // Simple choice: LocalDB test database (Windows dev)
            // If you don't have LocalDB, change to your SQL Server instance.
            _connString =
                "Server=(localdb)\\MSSQLLocalDB;Database=LeadRoutePlanner_Test;Trusted_Connection=True;TrustServerCertificate=True";
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Testing");

            builder.ConfigureServices(services =>
            {
                // Remove existing AppDbContext registration
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
                if (descriptor is not null)
                    services.Remove(descriptor);

                services.AddDbContext<AppDbContext>(opt =>
                {
                    opt.UseSqlServer(_connString);
                });

                // Create DB (tables via EF mapping). Assumes your schema exists or EF can create it.
                // If you rely on SQL scripts only, see note below.
                using var sp = services.BuildServiceProvider();
                using var scope = sp.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                db.Database.EnsureCreated();
            });
        }
    }
}
