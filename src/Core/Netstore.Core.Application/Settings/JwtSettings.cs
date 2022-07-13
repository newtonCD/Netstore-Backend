using Netstore.Application.Interfaces;

namespace Netstore.Application.Settings;

public class JwtSettings : IAppSettings
{
    public string Key { get; set; }
    public int TokenExpirationInMinutes { get; set; }
    public int RefreshTokenExpirationInDays { get; set; }
}