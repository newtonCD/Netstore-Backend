using Ardalis.GuardClauses;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Netstore.Infrastructure.Extensions;

public static class CachingExtensions
{
    public static async Task<T> GetAsync<T>(this IDistributedCache distributedCache, string cacheKey, CancellationToken token = default)
    {
        Guard.Against.Null(distributedCache, nameof(distributedCache));
        Guard.Against.Null(cacheKey, nameof(cacheKey));
        byte[] utf8Bytes = await distributedCache.GetAsync(cacheKey, token).ConfigureAwait(continueOnCapturedContext: false);
        if (utf8Bytes != null)
        {
            return JsonSerializer.Deserialize<T>(utf8Bytes);
        }

        return default;
    }

    public static async Task RemoveAsync(this IDistributedCache distributedCache, string cacheKey, CancellationToken token = default)
    {
        Guard.Against.Null(distributedCache, nameof(distributedCache));
        Guard.Against.Null(cacheKey, nameof(cacheKey));
        await distributedCache.RemoveAsync(cacheKey, token).ConfigureAwait(continueOnCapturedContext: false);
    }

    public static async Task SetAsync<T>(this IDistributedCache distributedCache, string cacheKey, T obj, int cacheExpirationInMinutes = 30, CancellationToken token = default)
    {
        Guard.Against.Null(distributedCache, nameof(distributedCache));
        Guard.Against.Null(cacheKey, nameof(cacheKey));
        Guard.Against.Null(obj, nameof(obj));
        DistributedCacheEntryOptions options = new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(cacheExpirationInMinutes));
        byte[] utf8Bytes = JsonSerializer.SerializeToUtf8Bytes(obj);
        await distributedCache.SetAsync(cacheKey, utf8Bytes, options, token).ConfigureAwait(continueOnCapturedContext: false);
    }
}