using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Netstore.Core.Application.Enums;
using Netstore.Core.Application.Extensions;
using Netstore.Core.Application.Interfaces.Repositories;
using Netstore.Core.Application.Interfaces.Services;
using Netstore.Core.Application.Services;
using Netstore.Core.Application.Settings;
using Netstore.Infrastructure.DbContexts;
using Netstore.Infrastructure.Identity.Models;
using Netstore.Infrastructure.Repositories;
using StackExchange.Redis;
using System;

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
    public static IServiceCollection ConfigureInfrastructureLayer(this IServiceCollection services, IConfiguration config, CoreServices coreServices = CoreServices.All)
    {
        _coreServices = coreServices;
        _config = config;

        services.AddDatabaseContext();
        services.AddRepositories();
        services.AddSharedInfrastructure();

        return services;
    }

    /// <summary>
    /// Adds the database context.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <returns></returns>
    private static IServiceCollection AddDatabaseContext(this IServiceCollection services)
    {
        DatabaseSettings databaseSettings = _config.GetSection(nameof(DatabaseSettings)).Get<DatabaseSettings>();

        if (databaseSettings.UseInMemoryDatabase)
        {
            services.AddDbContext<IdentityContext>(options =>
                options.UseInMemoryDatabase("IdentityDb"));
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase("ApplicationDb"));
        }
        else
        {
            services.AddDbContext<IdentityContext>(options =>
                options.UseSqlServer(databaseSettings.IdentityConnection));
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(databaseSettings.ApplicationConnection, b =>
                    b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
        }

        services.AddIdentity<ApplicationUser, IdentityRole>(options =>
        {
            options.SignIn.RequireConfirmedAccount = true;
            options.Password.RequireNonAlphanumeric = false;
        }).AddEntityFrameworkStores<IdentityContext>().AddDefaultTokenProviders();

        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddTransient(typeof(IRepositoryAsync<>), typeof(RepositoryAsync<>));

        return services;
    }

    /// <summary>
    /// Adds the API infrastructure.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <returns></returns>
    private static IServiceCollection AddSharedInfrastructure(this IServiceCollection services)
    {
        services.AddDynamicServiceRegistration();

        if (_coreServices.HasFlag(CoreServices.All) || _coreServices.HasFlag(CoreServices.Cache))
            services.AddCacheService();

        return services;
    }

    /// <summary>
    /// Adds the cache service.
    /// </summary>
    /// <param name="services">The services.</param>
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