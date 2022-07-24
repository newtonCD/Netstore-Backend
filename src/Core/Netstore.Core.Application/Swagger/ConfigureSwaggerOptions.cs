using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Netstore.Core.Application.Settings;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;

namespace Netstore.Core.Application.Swagger;

[ExcludeFromCodeCoverage]
public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _provider;
    private readonly IConfiguration _configuration;

    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigureSwaggerOptions"/> class.
    /// </summary>
    /// <param name="provider">The provider.</param>
    /// <param name="configuration">The configuration.</param>
    public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider, IConfiguration configuration)
    {
        _provider = provider;
        _configuration = configuration;
    }

    /// <summary>
    /// Invoked to configure a SwaggerGenOptions instance.
    /// </summary>
    /// <param name="options">The options instance to configure.</param>
    public void Configure(SwaggerGenOptions options)
    {
        foreach (var description in _provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
        }

        options.CustomSchemaIds(type => type.ToString());

        string xmlFilename = $"{Assembly.GetEntryAssembly().GetName().Name}.xml";

        try
        {
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
        }
        catch
        {
            // ignore
        }
    }

    /// <summary>
    /// Creates the information for API version.
    /// </summary>
    /// <param name="description">The description.</param>
    /// <returns></returns>
    private OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
    {
        SwaggerSettings swaggerSettings = _configuration.GetSection(nameof(SwaggerSettings)).Get<SwaggerSettings>();

        var info = new OpenApiInfo()
        {
            Title = swaggerSettings.Title,
            Description = swaggerSettings.Description,
            Version = description.ApiVersion.ToString(),
            Contact = new OpenApiContact()
            {
                Name = swaggerSettings.ContactName,
                Email = swaggerSettings.ContactEmail
            }
        };

        if (description.IsDeprecated)
        {
            info.Description += swaggerSettings.DeprecatedMessage;
        }

        return info;
    }
}