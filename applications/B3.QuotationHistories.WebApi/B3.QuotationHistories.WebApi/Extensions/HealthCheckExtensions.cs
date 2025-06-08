using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace B3.QuotationHistories.WebApi.Extensions;

public static class HealthCheckExtensions
{
    public static IEndpointConventionBuilder MapHealthChecks(this IEndpointRouteBuilder app, string path = "/health")
    {
        return app.MapHealthChecks(path, new HealthCheckOptions
        {
            ResponseWriter = async (context, report) =>
            {
                context.Response.ContentType = "application/json";

                var result = JsonSerializer.Serialize(new
                {
                    status = report.Status.ToString(),
                    totalDuration = $"{report.TotalDuration.TotalMilliseconds} ms",
                    checks = report.Entries.Select(entry => new
                    {
                        name = entry.Key,
                        status = entry.Value.Status.ToString(),
                        exception = entry.Value.Exception?.Message ?? "none",
                        duration = $"{entry.Value.Duration.TotalMilliseconds} ms"
                    })
                });

                await context.Response.WriteAsync(result);
            }
        });
    }
}