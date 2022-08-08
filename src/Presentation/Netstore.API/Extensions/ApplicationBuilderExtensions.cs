using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Netstore.Core.Application.Enums;
using Netstore.Core.Application.Settings;
using Serilog;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Netstore.API.Extensions;

[ExcludeFromCodeCoverage]
public static class ApplicationBuilderExtensions
{
    /// <summary>
    /// Configures the web application.
    /// </summary>
    /// <param name="app">The application.</param>
    /// <param name="config">The configuration.</param>
    /// <param name="coreServices">The core services.</param>
    public static IApplicationBuilder ConfigureWebApiApplication(this WebApplication app, IConfiguration config, CoreServices coreServices = CoreServices.All)
    {
        const string CorsPoliceName = "CorsPolicy";

        app.UseExceptionHandler("/error");
        app.UseHttpsRedirection();

        if ((coreServices.HasFlag(CoreServices.All) || coreServices.HasFlag(CoreServices.IpRateLimit))
            && config.GetSection(nameof(IpRateLimitSettings)).Get<IpRateLimitSettings>().EnableEndpointRateLimiting)
        {
            app.UseIpRateLimiting();
        }

        if ((coreServices.HasFlag(CoreServices.All) || coreServices.HasFlag(CoreServices.Swagger))
            && config.GetSection(nameof(SwaggerSettings)).Get<SwaggerSettings>().Enabled)
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                IApiVersionDescriptionProvider provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
                foreach (string description in provider.ApiVersionDescriptions.Select(x => x.GroupName).Where(x => x != null))
                {
                    options.SwaggerEndpoint($"/swagger/{description}/swagger.json", description.ToUpperInvariant());
                }

                options.RoutePrefix = "swagger";
                options.DisplayRequestDuration();
            });

            app.UseSerilogRequestLogging();
        }

        app.UseRouting();

        if (coreServices.HasFlag(CoreServices.All) || coreServices.HasFlag(CoreServices.Cors))
            app.UseCors(CorsPoliceName);

        app.UseAuthentication();
        app.UseAuthorization();

        if (coreServices.HasFlag(CoreServices.All) || coreServices.HasFlag(CoreServices.HealthChecks))
            app.UseHealthChecks("/health");

        app.UseEndpoints(endpoints =>
        {
            if (coreServices.HasFlag(CoreServices.All) || coreServices.HasFlag(CoreServices.Cors))
                endpoints.MapControllers().RequireCors(CorsPoliceName);
            else
                endpoints.MapControllers();

            if (coreServices.HasFlag(CoreServices.All) || coreServices.HasFlag(CoreServices.HealthChecks))
            {
                endpoints.MapHealthChecks("/ready", new HealthCheckOptions()
                {
                    Predicate = (check) => check.Tags.Contains("ready"),
                });
                endpoints.MapHealthChecks("/live", new HealthCheckOptions()
                {
                    Predicate = (_) => false
                });
            }
        });

        return app;
    }
}