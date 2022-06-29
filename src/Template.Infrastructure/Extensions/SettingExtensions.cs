using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;
using Template.Application.Settings;

namespace Template.Infrastructure.Extensions;

[ExcludeFromCodeCoverage]
public static class SettingExtensions
{
    internal static IServiceCollection AddSettings(this IServiceCollection services, IConfiguration config)
    {
        return services
            .Configure<CacheSettings>(config.GetSection(nameof(CacheSettings)))
            .Configure<IpRateLimitSettings>(config.GetSection(nameof(IpRateLimitSettings)))
            .Configure<JwtSettings>(config.GetSection(nameof(JwtSettings)))
            .Configure<SwaggerSettings>(config.GetSection(nameof(SwaggerSettings)));
    }
}