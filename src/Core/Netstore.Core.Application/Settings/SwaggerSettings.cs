using Netstore.Core.Application.Interfaces;

namespace Netstore.Core.Application.Settings;

public class SwaggerSettings : IAppSettings
{
    public string ContactEmail { get; set; } = string.Empty;
    public string ContactName { get; set; } = string.Empty;
    public string ContactUrl { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string DeprecatedMessage { get; set; } = string.Empty;
    public bool Enable { get; set; }
    public string Title { get; set; } = string.Empty;
}