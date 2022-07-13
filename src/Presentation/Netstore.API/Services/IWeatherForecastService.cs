using Netstore.Core.Application.Interfaces;
using Netstore.Core.Application.Interfaces.Services;
using Netstore.API.Model;

namespace Netstore.API.Services;

public interface IWeatherForecastService : IScopedService
{
    WeatherForecast Get();
}
