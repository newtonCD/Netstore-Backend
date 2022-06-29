using Template.Application.Interfaces;

namespace Template.Application.Settings;

public class DatabaseSettings : IAppSettings
{
    public string DBProvider { get; set; }
    public string ConnectionString { get; set; }
}