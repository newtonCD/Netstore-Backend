using Netstore.Core.Application.Interfaces;

namespace Netstore.Core.Application.Settings;

public class DatabaseSettings : IAppSettings
{
    public bool UseInMemoryDatabase { get; set; }
    public string ApplicationConnection { get; set; }
    public string IdentityConnection { get; set; }
}