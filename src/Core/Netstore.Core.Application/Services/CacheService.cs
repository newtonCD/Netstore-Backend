using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Netstore.Core.Application.Interfaces.Services;
using Netstore.Core.Application.Settings;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace Netstore.Core.Application.Services;

[ExcludeFromCodeCoverage]
public class CacheService : ICacheService
{
    private readonly ILogger<CacheService> _logger;
    private readonly IDistributedCache _cache;
    private readonly IConfiguration _configuration;

    /// <summary>
    /// Initializes a new instance of the <see cref="CacheService"/> class.
    /// </summary>
    /// <param name="cache">The cache.</param>
    /// <param name="logger">The logger.</param>
    /// <param name="configuration">The configuration.</param>
    public CacheService(
        IDistributedCache cache,
        ILogger<CacheService> logger,
        IConfiguration configuration)
    {
        _cache = cache;
        _logger = logger;
        _configuration = configuration;
    }

    /// <summary>
    /// Gets the specified key.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <returns></returns>
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

    /// <summary>
    /// Gets the asynchronous.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <param name="token">The token.</param>
    /// <returns></returns>
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

    /// <summary>
    /// Refreshes the specified key.
    /// </summary>
    /// <param name="key">The key.</param>
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

    /// <summary>
    /// Refreshes the asynchronous.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <param name="token">The token.</param>
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

    /// <summary>
    /// Removes the specified key.
    /// </summary>
    /// <param name="key">The key.</param>
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

    /// <summary>
    /// Removes the asynchronous.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <param name="token">The token.</param>
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

    /// <summary>
    /// Sets the specified key.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <param name="value">The value.</param>
    /// <param name="options">The options.</param>
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

    /// <summary>
    /// Sets the asynchronous.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <param name="value">The value.</param>
    /// <param name="options">The options.</param>
    /// <param name="token">The token.</param>
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