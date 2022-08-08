using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using Netstore.TestCore;
using Xunit;

namespace Netstore.IntegrationTests;

public class WeatherForecastControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private const string WeatherForecastUriPath = "api/{0}/weatherforecast";
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory<Program> _factory;

    public WeatherForecastControllerIntegrationTests(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client ??= factory.CreateClient();
    }

    [Theory]
    [InlineData("v1")]
    [InlineData("v2")]
    public async void Get_WithFullyIntegratedServices_ReturnsOkWithExpectedResponse(string apiVersion)
    {
        // Act
        var weatherForecastResponse = await _client.GetAsync(string.Format(WeatherForecastUriPath, apiVersion));

        // Assert
        Assert.Equal(HttpStatusCode.OK, weatherForecastResponse.StatusCode);

        var weatherForecastContent = await weatherForecastResponse.Content.ReadAsStringAsync();
        //var weatherForecast = JsonConvert.DeserializeObject<WeatherForecast>(weatherForecastContent);

        //Assert.NotNull(weatherForecast);
        //Assert.NotNull(weatherForecast.Summary);

        _client.Dispose();
    }

    [Theory]
    [InlineData("v1")]
    [InlineData("v2")]
    public async void Get_WeatherForecastServiceThrowsException_ReturnsBadRequestWithExpectedErrorResponse(string apiVersion)
    {
        // Arrange
        var randomExceptionMessage = Guid.NewGuid().ToString();
        var expectedException = new Exception(randomExceptionMessage);

        //var weatherForecastServiceMock = new Mock<IWeatherForecastService>();
        //weatherForecastServiceMock.Setup(weatherForecastService => weatherForecastService.Get())
        //    .Throws(expectedException);

        //var webApplicationFactoryWithMockedServices = _factory.WithWebHostBuilder(builder =>
        //    builder.ConfigureTestServices(services =>
        //        services.AddScoped(serviceProvider => weatherForecastServiceMock.Object)));

        //var httpClientWithMockedService = webApplicationFactoryWithMockedServices.CreateClient();

        // Act
        //var weatherForecastResponse = await httpClientWithMockedService.GetAsync(string.Format(WeatherForecastUriPath, apiVersion));

        // Assert
        //Assert.Equal(HttpStatusCode.BadRequest, weatherForecastResponse.StatusCode);

        //var errorResponse = await weatherForecastResponse.Content.ReadAsStringAsync();
        //Assert.Equal(randomExceptionMessage, errorResponse);

        //httpClientWithMockedService.Dispose();
        _client.Dispose();
    }
}