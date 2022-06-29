using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Template.Application.Interfaces;

namespace Template.Infrastructure.DependencyInjection;

[ExcludeFromCodeCoverage]
public static class DynamicServiceRegistrationExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        Type transientServiceType = typeof(ITransientService);
        Type scopedServiceType = typeof(IScopedService);
        var transientServices = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(p => transientServiceType.IsAssignableFrom(p))
            .Where(t => t.IsClass && !t.IsAbstract)
            .Select(t => new
            {
                Service = t.GetInterfaces().FirstOrDefault(),
                Implementation = t
            })
            .Where(t => t.Service != null);

        var scopedServices = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(p => scopedServiceType.IsAssignableFrom(p))
            .Where(t => t.IsClass && !t.IsAbstract)
            .Select(t => new
            {
                Service = t.GetInterfaces().FirstOrDefault(),
                Implementation = t
            })
            .Where(t => t.Service != null);

        foreach (var transientService in transientServices.Where(a => a != null))
        {
            if (transientServiceType.IsAssignableFrom(transientService.Service))
                services.AddTransient(transientService.Service, transientService.Implementation);
        }

        foreach (var scopedService in scopedServices.Where(a => a != null))
        {
            if (scopedServiceType.IsAssignableFrom(scopedService.Service))
                services.AddScoped(scopedService.Service, scopedService.Implementation);
        }

        return services;
    }
}