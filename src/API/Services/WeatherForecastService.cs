using Netstore.API.Model;

namespace Netstore.API.Services;

public class WeatherForecastService : IWeatherForecastService
{
    public WeatherForecast Get()
    {
        return new WeatherForecast
        {
            Summary = nameof(WeatherForecast)
        };
    }
}