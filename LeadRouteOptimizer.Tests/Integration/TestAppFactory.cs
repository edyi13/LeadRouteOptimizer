using LeadRouteOptimizer.Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LeadRouteOptimizer.Tests.Integration
{
    public class TestAppFactory : WebApplicationFactory<Program>
    {
        private readonly string _connString;

        public TestAppFactory()
        {
            // use a separate database for testing
            _connString =
                "Server=(localdb)\\MSSQLLocalDB;Database=LeadRoutePlanner_Test;Trusted_Connection=True;TrustServerCertificate=True";
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Testing");

            builder.ConfigureServices(services =>
            {
                // remove the existing AppDbContext registration
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
                if (descriptor is not null)
                    services.Remove(descriptor);

                services.AddDbContext<AppDbContext>(opt =>
                {
                    opt.UseSqlServer(_connString);
                });

                // create the database schema, assuming EF Migrations are used.
                // if not, use db.Database.EnsureCreated() instead.
                using var sp = services.BuildServiceProvider();
                using var scope = sp.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                db.Database.EnsureCreated();
            });
        }
    }
}
