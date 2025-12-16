using LeadRouteOptimizer.Api.Endpoints;
using LeadRouteOptimizer.Application.Plans.Commands;
using LeadRouteOptimizer.Application.Uploads.Commands;
using LeadRouteOptimizer.Application.Uploads.Queries;
using LeadRouteOptimizer.Infrastructure.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddScoped<UploadLeadsHandler>();
builder.Services.AddScoped<CreatePlanHandler>();
builder.Services.AddScoped<GetPlanHandler>();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "LeadRouteOptimizer API",
        Version = "v1"
    });

    options.MapType<IFormFile>(() => new Microsoft.OpenApi.Models.OpenApiSchema
    {
        Type = "string",
        Format = "binary"
    });
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "LeadRouteOptimizer API v1");
    });
}

app.UseAuthorization();

app.MapControllers();

app.Run();
public partial class Program { }