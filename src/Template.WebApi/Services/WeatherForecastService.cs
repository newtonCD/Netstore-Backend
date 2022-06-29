using Template.WebApi.Model;

namespace Template.WebApi.Services;

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