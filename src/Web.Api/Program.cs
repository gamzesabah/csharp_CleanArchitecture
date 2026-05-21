using System.Reflection;
using Application;
using HealthChecks.UI.Client;
using Infrastructure;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Serilog;
using Web.Api;
using Web.Api.Extensions;
using Web.Api.Middleware;

WebApplicationBuilder builder =
    WebApplication.CreateBuilder(args);

builder.Host.UseSerilog(
    (context, loggerConfig) =>
        loggerConfig.ReadFrom.Configuration(
            context.Configuration));

builder.Services.AddSwaggerGenWithAuth();

builder.Services
    .AddApplication()
    .AddPresentation()
    .AddInfrastructure(
        builder.Configuration);

builder.Services
    .AddHealthChecks()
    .AddNpgSql(
        builder.Configuration
            .GetConnectionString(
                "Database")!);

builder.Services.AddEndpoints(
    Assembly.GetExecutingAssembly());

WebApplication app =
    builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerWithUi();

    await app.ApplyMigrationsAndSeedDataAsync();
}

app.MapHealthChecks(
    "/health",
    new HealthCheckOptions
    {
        Predicate = _ => false
    });

app.MapHealthChecks(
    "/health/ready",
    new HealthCheckOptions
    {
        Predicate = _ => true,
        ResponseWriter =
            UIResponseWriter.WriteHealthCheckUIResponse
    });

app.UseRequestContextLogging();

app.UseMiddleware<CorrelationIdMiddleware>();

app.UseSerilogRequestLogging();

app.UseExceptionHandler();

app.UseAuthentication();

app.UseMiddleware<IdempotencyMiddleware>();

app.UseAuthorization();

// REMARK: If you want to use Controllers, you'll need this.
app.MapControllers();

app.MapEndpoints();

await app.RunAsync();

// REMARK: Required for functional and integration tests to work.
namespace Web.Api
{
    public partial class Program;
}
