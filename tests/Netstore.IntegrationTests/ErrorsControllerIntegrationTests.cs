using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using Netstore.TestCore;
using Xunit;

namespace Netstore.IntegrationTests;

public class ErrorsControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory<Program> _factory;

    public ErrorsControllerIntegrationTests(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client ??= factory.CreateClient();
    }

    [Fact]
    public async void Get_WithFullyIntegratedServices_ReturnsError()
    {
        // Act
        var errorsResponse = await _client.GetAsync("/error");

        // Assert
        Assert.Equal(HttpStatusCode.InternalServerError, errorsResponse.StatusCode);
        var errorContent = await errorsResponse.Content.ReadAsStringAsync();
        var problemDetail = JsonConvert.DeserializeObject<ProblemDetails>(errorContent);

        Assert.NotNull(problemDetail);

        _client.Dispose();
    }
}