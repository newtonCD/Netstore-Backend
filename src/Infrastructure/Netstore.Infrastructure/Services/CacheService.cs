using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Netstore.Core.Application.Interfaces.Services;
using Netstore.Core.Application.Settings;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace Netstore.Infrastructure.Services;

[ExcludeFromCodeCoverage]
public class CacheService : ICacheService
{
    private readonly ILogger<CacheService> _logger;
    private readonly IDistributedCache _cache;
    private readonly IConfiguration _configuration;

    public CacheService(
        IDistributedCache cache,
        ILogger<CacheService> logger,
        IConfiguration configuration)
    {
        _cache = cache;
        _logger = logger;
        _configuration = configuration;
    }

    public byte[] Get(string key)
    {
        try
        {
            return _cache.Get(key);
        }
        catch
        {
            return Array.Empty<byte>();
        }
    }

    public async Task<byte[]> GetAsync(string key, CancellationToken token = default)
    {
        try
        {
            return await _cache.GetAsync(key, token);
        }
        catch
        {
            return Array.Empty<byte>();
        }
    }

    public void Refresh(string key)
    {
        try
        {
            _cache.Refresh(key);
        }
        catch
        {
            // Ignore
        }
    }

    public async Task RefreshAsync(string key, CancellationToken token = default)
    {
        try
        {
            await _cache.RefreshAsync(key, token);
            _logger.LogDebug("Cache Refreshed : {key}", key);
        }
        catch
        {
            // Ignore
        }
    }

    public void Remove(string key)
    {
        try
        {
            _cache.Remove(key);
        }
        catch
        {
            // Ignore
        }
    }

    public async Task RemoveAsync(string key, CancellationToken token = default)
    {
        try
        {
            await _cache.RemoveAsync(key, token);
        }
        catch
        {
            // Ignore
        }
    }

    public void Set(string key, byte[] value, DistributedCacheEntryOptions options)
    {
        try
        {
            options.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(_configuration.GetSection(nameof(CacheSettings)).Get<CacheSettings>().AbsoluteExpirationInSeconds);
            options.SlidingExpiration = TimeSpan.FromSeconds(_configuration.GetSection(nameof(CacheSettings)).Get<CacheSettings>().SlidingExpirationInSeconds);

            _cache.Set(key, value, options);
            _logger.LogDebug("Added to Cache : {key}", key);
        }
        catch
        {
            // Ignore
        }
    }

    public async Task SetAsync(string key, byte[] value, DistributedCacheEntryOptions options, CancellationToken token = default)
    {
        try
        {
            options.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(_configuration.GetSection(nameof(CacheSettings)).Get<CacheSettings>().AbsoluteExpirationInSeconds);
            options.SlidingExpiration = TimeSpan.FromSeconds(_configuration.GetSection(nameof(CacheSettings)).Get<CacheSettings>().SlidingExpirationInSeconds);

            await _cache.SetAsync(key, value, options, token);
            _logger.LogDebug("Added to Cache : {key}", key);
        }
        catch
        {
            // Ignore
        }
    }
}