using Template.Application.Interfaces;

namespace Template.Application.Settings;

public class CacheSettings : IAppSettings
{
    public bool Enabled { get; set; }
    public bool PreferRedis { get; set; }
    public string RedisURL { get; set; }
    public int AbsoluteExpirationInSeconds { get; set; }
    public int SlidingExpirationInSeconds { get; set; }
}