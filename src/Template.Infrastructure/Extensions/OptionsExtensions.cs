using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Template.Infrastructure.Extensions;

[ExcludeFromCodeCoverage]
public static class OptionsExtensions
{
    public static T GetOptions<T>(this IServiceCollection services, string sectionName)
        where T : new()
    {
        using ServiceProvider serviceProvider = services.BuildServiceProvider();
        IConfiguration configuration = serviceProvider.GetRequiredService<IConfiguration>();
        IConfigurationSection section = configuration.GetSection(sectionName);

        var options = new T();
        section.Bind(options);

        return options;
    }
}