using AspNetCoreRateLimit;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Netstore.Core.Application.Common.Behaviours;
using Netstore.Core.Application.Enums;
using Netstore.Core.Application.Interfaces.Services;
using Netstore.Core.Application.Services;
using Netstore.Core.Application.Settings;
using Netstore.Core.Application.Swagger;
using StackExchange.Redis;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text;

namespace Netstore.Core.Application.DependencyInjection;

[ExcludeFromCodeCoverage]
public static class ServiceCollectionExtension
{
    private static CoreServices _coreServices;
    private static IConfiguration _config;

    /// <summary>
    /// Configures the specified configuration.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <param name="config">The configuration.</param>
    /// <param name="coreServices">The core services.</param>
    /// <returns></returns>
    public static IServiceCollection Configure(this IServiceCollection services, IConfiguration config, CoreServices coreServices = CoreServices.All)
    {
        _coreServices = coreServices;
        _config = config;

        services.AddSettings();
        services.AddApplication();
        services.AddInfrastructure();

        return services;
    }

    /// <summary>
    /// Adds the settings.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <returns></returns>
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

    /// <summary>
    /// Adds the application.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <returns></returns>
    private static IServiceCollection AddApplication(this IServiceCollection services)
    {
        if (_coreServices.HasFlag(CoreServices.All) || _coreServices.HasFlag(CoreServices.AutoMapper))
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

        if (_coreServices.HasFlag(CoreServices.All) || _coreServices.HasFlag(CoreServices.FluentValidation))
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        if (_coreServices.HasFlag(CoreServices.All) || _coreServices.HasFlag(CoreServices.MediatR))
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.TryAddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
            services.TryAddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        }

        if (_coreServices.HasFlag(CoreServices.All) || _coreServices.HasFlag(CoreServices.Cors))
        {
            services.AddCors(co =>
            {
                co.AddPolicy("CorsPolicy", cpb =>
                {
                    cpb.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });
        }

        return services;
    }

    /// <summary>
    /// Adds the infrastructure.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <returns></returns>
    private static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        if (_coreServices.HasFlag(CoreServices.All) || _coreServices.HasFlag(CoreServices.Cache))
            services.AddCacheService();

        if (_coreServices.HasFlag(CoreServices.All) || _coreServices.HasFlag(CoreServices.IpRateLimit))
            services.AddIpRatingLimit();

        if (_coreServices.HasFlag(CoreServices.All) || _coreServices.HasFlag(CoreServices.FluentValidation))
            services.AddControllers().AddFluentValidation();

        if (_coreServices.HasFlag(CoreServices.All) || _coreServices.HasFlag(CoreServices.Swagger))
            services.AddSwaggerAndApiVersioning();

        if (_coreServices.HasFlag(CoreServices.All) || _coreServices.HasFlag(CoreServices.HealthChecks))
            services.AddHealthChecks();

        services.AddDynamicServiceRegistration();
        services.AddMvc();
        services.AddRouting(options =>
        {
            options.LowercaseUrls = true;
        });

        if (_coreServices.HasFlag(CoreServices.All) || _coreServices.HasFlag(CoreServices.JsonWebToken))
            services.AddJwtAuthentication();

        return services;
    }

    /// <summary>
    /// Adds the ip rating limit.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <returns></returns>
    private static IServiceCollection AddIpRatingLimit(this IServiceCollection services)
    {
        IpRateLimitSettings ipRateLimitSettings = _config.GetSection(nameof(IpRateLimitSettings)).Get<IpRateLimitSettings>();

        if (!ipRateLimitSettings.EnableEndpointRateLimiting)
            return services;

        services.AddMemoryCache();
        services.Configure<IpRateLimitOptions>(_config.GetSection(nameof(IpRateLimitSettings)));
        services.Configure<IpRateLimitPolicies>(_config.GetSection(nameof(IpRateLimitPolicies)));
        services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
        services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

        if (ipRateLimitSettings.IsAppInLoadBalance)
        {
            services.AddSingleton<IIpPolicyStore, DistributedCacheIpPolicyStore>();
            services.AddSingleton<IRateLimitCounterStore, DistributedCacheRateLimitCounterStore>();
        }
        else
        {
            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
        }

        return services;
    }

    /// <summary>
    /// Adds the swagger and API versioning.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <returns></returns>
    private static IServiceCollection AddSwaggerAndApiVersioning(this IServiceCollection services)
    {
        if (!_config.GetSection(nameof(SwaggerSettings)).Get<SwaggerSettings>().Enabled)
            return services;

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.ExampleFilters();
            options.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();
            options.OperationFilter<SecurityRequirementsOperationFilter>(true, "Bearer");
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                BearerFormat = "JWT",
                Description = "Standard Authorization header using the Bearer scheme (JWT). Example: \"bearer {token}\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
            });
        });

        services.AddSwaggerExamplesFromAssemblies(Assembly.GetEntryAssembly());

        if (_coreServices.HasFlag(CoreServices.All) || _coreServices.HasFlag(CoreServices.FluentValidation))
            services.AddFluentValidationRulesToSwagger();

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

    /// <summary>
    /// Adds the JWT authentication.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <returns></returns>
    private static IServiceCollection AddJwtAuthentication(this IServiceCollection services)
    {
        JwtSettings jwtSettings = _config.GetSection(nameof(JwtSettings)).Get<JwtSettings>();

        if (!jwtSettings.Enabled)
            return services;

        services.AddAuthentication(opt =>
        {
            opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings.ValidIssuer,
                ValidAudience = jwtSettings.ValidAudience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
            };
        });
        services.AddAuthorization();

        return services;
    }

    /// <summary>
    /// Adds the cache service.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <returns></returns>
    private static IServiceCollection AddCacheService(this IServiceCollection services)
    {
        CacheSettings cacheSettings = _config.GetSection(nameof(CacheSettings)).Get<CacheSettings>();

        if (!cacheSettings.Enabled)
            return services;

        if (cacheSettings.PreferRedis && !string.IsNullOrWhiteSpace(cacheSettings.RedisConnectionString))
        {
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = cacheSettings.RedisConnectionString;
                options.ConfigurationOptions = new ConfigurationOptions()
                {
                    AbortOnConnectFail = cacheSettings.RedisConfigurationOptionsAbortOnConnectFail,
                    ConnectRetry = cacheSettings.RedisConfigurationConnectRetry,
                    ReconnectRetryPolicy = new ExponentialRetry(
                                                Convert.ToInt32(TimeSpan.FromSeconds(cacheSettings.RedisExponentialRetryDeltaBackOffMilliseconds).TotalMilliseconds),
                                                Convert.ToInt32(TimeSpan.FromSeconds(cacheSettings.RedisExponentialRetryMaxDeltaBackOffMilliseconds).TotalMilliseconds)),
                    ConnectTimeout = cacheSettings.RedisConfigurationConnectTimeout,
                };
            });
        }
        else
        {
            services.AddDistributedMemoryCache();
        }

        services.TryAdd(ServiceDescriptor.Singleton<ICacheService, CacheService>());

        return services;
    }
}