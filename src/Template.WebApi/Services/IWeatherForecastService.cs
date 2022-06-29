using Template.Application.Interfaces;
using Template.WebApi.Model;

namespace Template.WebApi.Services;

public interface IWeatherForecastService : IScopedService
{
    WeatherForecast Get();
}
