using Netstore.Application.Interfaces;
using Netstore.Application.Interfaces.Services;
using Netstore.API.Model;

namespace Netstore.API.Services;

public interface IWeatherForecastService : IScopedService
{
    WeatherForecast Get();
}
