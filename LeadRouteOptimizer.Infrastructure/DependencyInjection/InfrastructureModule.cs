using LeadRouteOptimizer.Application.Interfaces;
using LeadRouteOptimizer.Infrastructure.Adapters;
using LeadRouteOptimizer.Infrastructure.Persistence;
using LeadRouteOptimizer.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LeadRouteOptimizer.Infrastructure.DependencyInjection
{
    public static class InfrastructureModule
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            var conn = config.GetConnectionString("Default")
                       ?? throw new InvalidOperationException("Missing ConnectionStrings:Default");

            services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(conn));

            services.AddScoped<IUploadBatchRepository, UploadBatchRepository>();
            services.AddScoped<ILeadRepository, LeadRepository>();
            services.AddScoped<IRoutePlanRepository, RoutePlanRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ILeadCsvParser, CsvLeadParser>();

            return services;
        }
    }
}
