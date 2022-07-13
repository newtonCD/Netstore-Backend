using Netstore.Core.Application.Interfaces;

namespace Netstore.Core.Application.Settings;

public class DatabaseSettings : IAppSettings
{
    public string DBProvider { get; set; }
    public string ConnectionString { get; set; }
}