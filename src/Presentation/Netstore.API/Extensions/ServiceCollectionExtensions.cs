using AspNetCoreRateLimit;
using FluentValidation.AspNetCore;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Netstore.Common.Results;
using Netstore.Core.Application.Enums;
using Netstore.Core.Application.Settings;
using Netstore.Core.Application.Swagger;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Reflection;
using System.Text;

namespace Netstore.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    private static CoreServices _coreServices;
    private static IConfiguration _config;

    /// <summary>
    /// Configures the infrastructure.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <param name="config">The configuration.</param>
    /// <param name="coreServices">The core services.</param>
    public static IServiceCollection ConfigureApiLayer(this IServiceCollection services, IConfiguration config, CoreServices coreServices = CoreServices.All)
    {
        _coreServices = coreServices;
        _config = config;

        if (_coreServices.HasFlag(CoreServices.All) || _coreServices.HasFlag(CoreServices.FluentValidation))
            services.AddControllers().AddFluentValidation();

        if (_coreServices.HasFlag(CoreServices.All) || _coreServices.HasFlag(CoreServices.IpRateLimit))
            services.AddIpRatingLimit();

        if (_coreServices.HasFlag(CoreServices.All) || _coreServices.HasFlag(CoreServices.Swagger))
            services.AddSwaggerAndApiVersioning();

        if (_coreServices.HasFlag(CoreServices.All) || _coreServices.HasFlag(CoreServices.HealthChecks))
            services.AddHealthChecks();

        services.AddMvc();
        services.AddRouting(options =>
        {
            options.LowercaseUrls = true;
        });

        if (_coreServices.HasFlag(CoreServices.All) || _coreServices.HasFlag(CoreServices.JsonWebToken))
            services.AddJwtAuthentication();

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
    /// Adds the ip rating limit.
    /// </summary>
    /// <param name="services">The services.</param>
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
    private static IServiceCollection AddJwtAuthentication(this IServiceCollection services)
    {
        JwtSettings jwtSettings = _config.GetSection(nameof(JwtSettings)).Get<JwtSettings>();

        if (!jwtSettings.Enabled)
            return services;

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(o =>
        {
            o.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ClockSkew = TimeSpan.Zero,
                ValidIssuer = jwtSettings.ValidIssuer,
                ValidAudience = jwtSettings.ValidAudience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
            };
            o.RequireHttpsMetadata = false;
            o.SaveToken = false;
            o.Events = new JwtBearerEvents()
            {
                OnAuthenticationFailed = c =>
                {
                    c.NoResult();
                    c.Response.StatusCode = 500;
                    c.Response.ContentType = "text/plain";
                    return c.Response.WriteAsync(c.Exception.ToString());
                },
                OnChallenge = context =>
                {
                    context.HandleResponse();
                    context.Response.StatusCode = 401;
                    context.Response.ContentType = "application/json";
                    string result = JsonConvert.SerializeObject(Result.Fail("You are not Authorized"));
                    return context.Response.WriteAsync(result);
                },
                OnForbidden = context =>
                {
                    context.Response.StatusCode = 403;
                    context.Response.ContentType = "application/json";
                    string result = JsonConvert.SerializeObject(Result.Fail("You are not authorized to access this resource"));
                    return context.Response.WriteAsync(result);
                },
            };
        });

        services.AddAuthorization();

        return services;
    }
}