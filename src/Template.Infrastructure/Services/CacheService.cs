using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Template.Application.Interfaces;

namespace Template.Infrastructure.Services;

[ExcludeFromCodeCoverage]
public class CacheService : ICacheService
{
    private readonly ILogger<CacheService> _logger;
    private readonly IDistributedCache _cache;

    public CacheService(IDistributedCache cache, ILogger<CacheService> logger)
    {
        _cache = cache;
        _logger = logger;
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
#pragma warning disable CA2254 // Template should be a static expression
            _logger.LogDebug($"Cache Refreshed : {key}");
#pragma warning restore CA2254 // Template should be a static expression
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
            _cache.Set(key, value, options);
#pragma warning disable CA2254 // Template should be a static expression
            _logger.LogDebug($"Added to Cache : {key}");
#pragma warning restore CA2254 // Template should be a static expression
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
            await _cache.SetAsync(key, value, options, token);
#pragma warning disable CA2254 // Template should be a static expression
            _logger.LogDebug($"Added to Cache : {key}");
#pragma warning restore CA2254 // Template should be a static expression
        }
        catch
        {
            // Ignore
        }
    }
}