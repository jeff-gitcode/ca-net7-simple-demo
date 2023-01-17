
using System.Net;
using System.Security.Claims;

using FluentAssertions;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

using WebMotions.Fake.Authentication.JwtBearer;

public class UsersController_IntegrationTest : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private const string Username = "Test";
    private const string Password = "test";

    private readonly string base64EncodedAuthenticationString = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes($"{Username}:{Password}"));

    public UsersController_IntegrationTest(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                services.AddAuthentication(FakeJwtBearerDefaults.AuthenticationScheme).AddFakeJwtBearer();

                // services.AddAuthorization(config =>
                // {
                //     config.DefaultPolicy = new AuthorizationPolicyBuilder(config.DefaultPolicy)
                //                                 .AddAuthenticationSchemes(BasicAuthenticationDefaults.AuthenticationScheme)
                //                                 .Build();
                // });
            });
        });
    }

    [Theory]
    [InlineData("/api/Users/")]
    public async Task Get_Users_Should_Return_Forbidden_Without_Authentication(string url)
    {
        // Arrange
        var client = _factory.CreateClient();
        var claims = new Dictionary<string, object>
            {
                { ClaimTypes.NameIdentifier, "test@test.com" },
            };
        client.SetFakeBearerToken(claims);
        // Act
        var response = await client.GetAsync(url);

        // Assert
        // response.EnsureSuccessStatusCode();
        response.Should().HaveStatusCode(HttpStatusCode.Forbidden);
    }
}