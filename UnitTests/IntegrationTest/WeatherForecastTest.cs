
using System.Net;

using FluentAssertions;

using Microsoft.AspNetCore.Mvc.Testing;

public class WeatherForecast_IntegrationTest : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public WeatherForecast_IntegrationTest(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Theory]
    [InlineData("/WeatherForecast")]
    public async Task Get_Weather_Endpoints_Should_Return_Success_And_CorrectContentType(string url)
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync(url);

        // Assert
        response.EnsureSuccessStatusCode();
        response.Should().HaveStatusCode(HttpStatusCode.OK);
    }
}