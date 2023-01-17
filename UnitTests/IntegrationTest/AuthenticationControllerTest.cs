
using System.Net;
using System.Net.Http.Json;

using Domain;

using FluentAssertions;

using Microsoft.AspNetCore.Mvc.Testing;

public class AuthenticationController_IntegrationTest : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public AuthenticationController_IntegrationTest(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Theory]
    [InlineData("/api/Authentication/register")]
    public async Task Get_Authentication_Register_Should_Return_Success_And_CorrectContentType(string url)
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.PostAsJsonAsync(url, new LoginDTO { Email = "john.doe.integration@email.com", Password = "P@ssword1", Role = "User", Token = "" });

        // Assert
        response.EnsureSuccessStatusCode();
        response.Should().HaveStatusCode(HttpStatusCode.OK);
    }

    [Theory]
    [InlineData("/api/Authentication/login")]
    public async Task Get_Authentication_Login_Should_Return_Success_And_CorrectContentType(string url)
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.PostAsJsonAsync(url, new LoginDTO { Email = "administrator@localhost", Password = "P@ssword1", Role = "Admin", Token = "" });

        // Assert
        response.EnsureSuccessStatusCode();
        response.Should().HaveStatusCode(HttpStatusCode.OK);
    }
}