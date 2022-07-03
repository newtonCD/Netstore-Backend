using Netstore.Application.Interfaces;

namespace Netstore.Application.Settings;

public class DatabaseSettings : IAppSettings
{
    public string DBProvider { get; set; }
    public string ConnectionString { get; set; }
}