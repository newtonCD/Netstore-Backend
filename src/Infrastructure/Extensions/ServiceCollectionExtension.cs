﻿using AspNetCoreRateLimit;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Netstore.Application.Settings;
using Netstore.Infrastructure.Swagger;
using System.Diagnostics.CodeAnalysis;

namespace Netstore.Infrastructure.Extensions;

[ExcludeFromCodeCoverage]
public static class ServiceCollectionExtension
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services.AddCacheService();

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

        services.AddControllers().AddFluentValidation();
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
}