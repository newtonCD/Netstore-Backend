using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Template.Application.Interfaces;
using Template.Application.Settings;
using Template.Infrastructure.Extensions;
using Template.Infrastructure.Services;

namespace Template.Infrastructure.Cache;

[ExcludeFromCodeCoverage]
public static class CacheExtensions
{
    public static IServiceCollection AddCacheService(this IServiceCollection services)
    {
        CacheSettings cacheSettings = services.GetOptions<CacheSettings>("CacheSettings");

        if (cacheSettings.Enabled)
        {
            if (cacheSettings.PreferRedis)
            {
                services.AddStackExchangeRedisCache(options =>
                {
                    options.Configuration = cacheSettings.RedisURL;
                    options.ConfigurationOptions = new StackExchange.Redis.ConfigurationOptions()
                    {
                        AbortOnConnectFail = true
                    };
                });
            }
            else
            {
                services.AddDistributedMemoryCache();
            }

            services.TryAdd(ServiceDescriptor.Singleton<ICacheService, CacheService>());
        }

        return services;
    }
}
