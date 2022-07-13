using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Netstore.Core.Application.Interfaces.Services;
using Netstore.Core.Application.Settings;
using Netstore.Infrastructure.Services;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Netstore.Infrastructure.Extensions;

[ExcludeFromCodeCoverage]
public static class CacheExtension
{
    public static IServiceCollection AddCacheService(this IServiceCollection services)
    {
        CacheSettings cacheSettings = services.GetOptions<CacheSettings>(nameof(CacheSettings));

        if (!cacheSettings.Enabled)
            return services;

        if (cacheSettings.PreferRedis && !string.IsNullOrWhiteSpace(cacheSettings.RedisConnectionString))
        {
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = cacheSettings.RedisConnectionString;
                options.ConfigurationOptions = new ConfigurationOptions()
                {
                    AbortOnConnectFail = true,
                    ConnectRetry = 5,
                    ReconnectRetryPolicy = new ExponentialRetry(
                                                    Convert.ToInt32(TimeSpan.FromSeconds(5).TotalMilliseconds),
                                                    Convert.ToInt32(TimeSpan.FromSeconds(10).TotalMilliseconds)),
                    ConnectTimeout = 1000,
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