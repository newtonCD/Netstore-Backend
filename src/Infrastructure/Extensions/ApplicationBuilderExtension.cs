#nullable enable

using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Netstore.Application.Settings;
using Serilog;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Netstore.Infrastructure.Extensions;

[ExcludeFromCodeCoverage]
public static class ApplicationBuilderExtension
{
    public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder app, IConfiguration config)
    {
        const string CorsPoliceName = "CorsPolicy";

        if (config.GetSection(nameof(IpRateLimitSettings)).Get<IpRateLimitSettings>().EnableEndpointRateLimiting)
            app.UseIpRateLimiting();

        app.UseExceptionHandler("/error");
        app.UseHttpsRedirection();
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            IApiVersionDescriptionProvider provider = app.ApplicationServices.GetRequiredService<IApiVersionDescriptionProvider>();
            foreach (string description in provider.ApiVersionDescriptions.Select(x => x.GroupName).Where(x => x != null))
            {
                options.SwaggerEndpoint($"/swagger/{description}/swagger.json", description.ToUpperInvariant());
            }
        });

        app.UseSerilogRequestLogging();
        app.UseRouting();
        app.UseCors(CorsPoliceName);
        app.UseAuthorization();
        app.UseHealthChecks("/health");
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers().RequireCors(CorsPoliceName);
            endpoints.MapHealthChecks("/ready", new HealthCheckOptions()
            {
                Predicate = (check) => check.Tags.Contains("ready"),
            });
            endpoints.MapHealthChecks("/live", new HealthCheckOptions()
            {
                Predicate = (_) => false
            });
        });

        return app;
    }
}