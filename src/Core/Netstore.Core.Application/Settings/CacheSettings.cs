using Netstore.Core.Application.Interfaces;
using System;

namespace Netstore.Core.Application.Settings;

public class CacheSettings : IAppSettings
{
    public bool Enabled { get; set; }
    public bool PreferRedis { get; set; }
    public string RedisConnectionString { get; set; }
    public int AbsoluteExpirationInSeconds { get; set; }
    public int SlidingExpirationInSeconds { get; set; }
    public bool RedisConfigurationOptionsAbortOnConnectFail { get; set; }
    public int RedisConfigurationConnectRetry { get; set; }
    public int RedisConfigurationConnectTimeout { get; set; }
    public int RedisExponentialRetryDeltaBackOffMilliseconds { get; set; }
    public int RedisExponentialRetryMaxDeltaBackOffMilliseconds { get; set; }
}