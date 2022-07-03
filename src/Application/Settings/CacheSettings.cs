using Netstore.Application.Interfaces;

namespace Netstore.Application.Settings;

public class CacheSettings : IAppSettings
{
    public bool Enabled { get; set; }
    public bool PreferRedis { get; set; }
    public string RedisURL { get; set; }
    public int AbsoluteExpirationInSeconds { get; set; }
    public int SlidingExpirationInSeconds { get; set; }
}