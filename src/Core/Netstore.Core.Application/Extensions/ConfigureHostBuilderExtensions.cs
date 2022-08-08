using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Netstore.Core.Application.Enums;
using Serilog;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Netstore.Core.Application.Extensions;

[ExcludeFromCodeCoverage]
public static class ConfigureHostBuilderExtensions
{
    /// <summary>
    /// Configures the specified core services.
    /// </summary>
    /// <param name="host">The host.</param>
    /// <param name="coreServices">The core services.</param>
    /// <returns></returns>
    public static ConfigureHostBuilder Configure(this ConfigureHostBuilder host, CoreServices coreServices = CoreServices.All)
    {
        host
            .AddConfigurationFiles()
            .AddSerilog(coreServices);

        return host;
    }

    /// <summary>
    /// Adds the configuration files.
    /// </summary>
    /// <param name="host">The host.</param>
    /// <returns></returns>
    private static ConfigureHostBuilder AddConfigurationFiles(this ConfigureHostBuilder host)
    {
        host.ConfigureAppConfiguration((context, config) =>
        {
            const string configurationsDirectory = "Configurations";
            IHostEnvironment env = context.HostingEnvironment;

            config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"{configurationsDirectory}/cache.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"{configurationsDirectory}/database.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"{configurationsDirectory}/ipratelimit.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"{configurationsDirectory}/jwt.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"{configurationsDirectory}/serilog.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"{configurationsDirectory}/swagger.json", optional: false, reloadOnChange: true)

                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"{configurationsDirectory}/cache.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"{configurationsDirectory}/database.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"{configurationsDirectory}/ipratelimit.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"{configurationsDirectory}/jwt.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"{configurationsDirectory}/serilog.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"{configurationsDirectory}/swagger.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);

            if (env.IsDevelopment())
            {
                config.AddUserSecrets(Assembly.GetExecutingAssembly(), optional: true, reloadOnChange: true);
            }

            config.AddEnvironmentVariables();
        });

        return host;
    }

    /// <summary>
    /// Adds the serilog.
    /// </summary>
    /// <param name="host">The host.</param>
    /// <param name="coreServices">The core services.</param>
    /// <returns></returns>
    private static ConfigureHostBuilder AddSerilog(this ConfigureHostBuilder host, CoreServices coreServices = CoreServices.All)
    {
        if (coreServices.HasFlag(CoreServices.All) || coreServices.HasFlag(CoreServices.Serilog))
        {
            host.UseSerilog((context, services, configuration) => configuration
                .ReadFrom.Configuration(context.Configuration)
                .ReadFrom.Services(services));
        }

        return host;
    }
}