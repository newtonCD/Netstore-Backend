using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Netstore.Core.Application.Enums;
using Netstore.Core.Application.Settings;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Netstore.Core.Application.Extensions;

[ExcludeFromCodeCoverage]
public static class ServiceCollectionExtensions
{
    private static CoreServices _coreServices;
    private static IConfiguration _config;

    /// <summary>
    /// Configures the specified configuration.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <param name="config">The configuration.</param>
    /// <param name="coreServices">The core services.</param>
    public static IServiceCollection ConfigureApplicationLayer(this IServiceCollection services, IConfiguration config, CoreServices coreServices = CoreServices.All)
    {
        _coreServices = coreServices;
        _config = config;

        services.AddApplication();
        services.AddSettings();

        return services;
    }

    /// <summary>
    /// Adds the application.
    /// </summary>
    /// <param name="services">The services.</param>
    private static IServiceCollection AddApplication(this IServiceCollection services)
    {
        if (_coreServices.HasFlag(CoreServices.All) || _coreServices.HasFlag(CoreServices.AutoMapper))
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

        if (_coreServices.HasFlag(CoreServices.All) || _coreServices.HasFlag(CoreServices.FluentValidation))
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        if (_coreServices.HasFlag(CoreServices.All) || _coreServices.HasFlag(CoreServices.MediatR))
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());
        }

        return services;
    }

    /// <summary>
    /// Adds the settings.
    /// </summary>
    /// <param name="services">The services.</param>
    private static IServiceCollection AddSettings(this IServiceCollection services)
    {
        if (_coreServices.HasFlag(CoreServices.All) || _coreServices.HasFlag(CoreServices.Cache))
            services.Configure<CacheSettings>(_config.GetSection(nameof(CacheSettings)));

        if (_coreServices.HasFlag(CoreServices.All) || _coreServices.HasFlag(CoreServices.IpRateLimit))
            services.Configure<IpRateLimitSettings>(_config.GetSection(nameof(IpRateLimitSettings)));

        if (_coreServices.HasFlag(CoreServices.All) || _coreServices.HasFlag(CoreServices.JsonWebToken))
            services.Configure<JwtSettings>(_config.GetSection(nameof(JwtSettings)));

        if (_coreServices.HasFlag(CoreServices.All) || _coreServices.HasFlag(CoreServices.Swagger))
            services.Configure<SwaggerSettings>(_config.GetSection(nameof(SwaggerSettings)));

        return services;
    }
}