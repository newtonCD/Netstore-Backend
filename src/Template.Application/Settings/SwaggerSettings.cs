using Template.Application.Interfaces;

namespace Template.Application.Settings;

public class SwaggerSettings : IAppSettings
{
    public bool Enable { get; set; }
    public string ContactName { get; set; }
    public string ContactEmail { get; set; }
    public string ContactUrl { get; set; }
}