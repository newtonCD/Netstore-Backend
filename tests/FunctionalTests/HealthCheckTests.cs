using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using System.Threading.Tasks;
using Netstore.TestCore;
using Xunit;

namespace Netstore.FunctionalTests;

public class HealthCheckTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory<Program> _factory;

    public HealthCheckTests(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
    }

    [Theory]
    [InlineData("/health")]
    [InlineData("/ready")]
    [InlineData("/live")]
    public async Task Get_HealthCheck(string value)
    {
        // Arrange
        var response = await _client.GetAsync($"{value}");

        // Act
        var responseContent = await response.Content.ReadAsStringAsync();

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal("Healthy", responseContent);

        _client.Dispose();
    }
}