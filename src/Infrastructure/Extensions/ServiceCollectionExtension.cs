using AspNetCoreRateLimit;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Netstore.Application.Settings;
using Netstore.Infrastructure.Swagger;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Netstore.Infrastructure.Extensions;

[ExcludeFromCodeCoverage]
public static class ServiceCollectionExtension
{
    public static IServiceCollection Configure(this IServiceCollection services, IConfiguration config)
    {
        services.AddApplication();
        services.AddInfrastructure(config);

        return services;
    }

    private static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services.AddCacheService();
        services.AddIpRatingLimit(config);
        services.AddControllers().AddFluentValidation();
        services.AddSwaggerAndApiVersioning();
        services.AddHealthChecks();
        services.AddMvc(x => x.EnableEndpointRouting = false);
        services.AddServices();
        services.AddSettings(config);
        services.AddRouting(options => options.LowercaseUrls = true);
        services.AddCors(co =>
            co.AddPolicy("CorsPolicy", cpb =>
                cpb.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

        return services;
    }

    private static IServiceCollection AddIpRatingLimit(this IServiceCollection services, IConfiguration config)
    {
        IpRateLimitSettings ipRateLimitSettings = config.GetSection(nameof(IpRateLimitSettings)).Get<IpRateLimitSettings>();

        // Rate Limit
        if (ipRateLimitSettings.EnableEndpointRateLimiting)
        {
            services.AddMemoryCache();
            services.Configure<IpRateLimitOptions>(config.GetSection(nameof(IpRateLimitSettings)));
            services.Configure<IpRateLimitPolicies>(config.GetSection(nameof(IpRateLimitPolicies)));
            services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

            // Caso esteja utilizando load-balance, deve-se alterar as implementações
            // abaixo para usar DistributedCacheIpPolicyStore e DistributedCacheRateLimitCounterStore
            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
        }

        return services;
    }

    private static IServiceCollection AddSwaggerAndApiVersioning(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.ConfigureOptions<ConfigureSwaggerOptions>();
        services.AddApiVersioning(option =>
        {
            option.DefaultApiVersion = new ApiVersion(1, 0);
            option.AssumeDefaultVersionWhenUnspecified = true;
            option.ReportApiVersions = true;
            option.ApiVersionReader = new UrlSegmentApiVersionReader();
        });
        services.AddVersionedApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });

        return services;
    }

    private static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddMediatR(Assembly.GetExecutingAssembly());

        return services;
    }
}