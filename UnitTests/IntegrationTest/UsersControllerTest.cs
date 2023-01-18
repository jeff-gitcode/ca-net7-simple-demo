
using System.Net;
using System.Security.Claims;

using Domain;

using FluentAssertions;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

using Newtonsoft.Json;

using WebMotions.Fake.Authentication.JwtBearer;

public class UsersController_IntegrationTest : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private const string Username = "Test";
    private const string Password = "test";

    private readonly string base64EncodedAuthenticationString = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes($"{Username}:{Password}"));
    private UserDTO _currentUser;
    public UsersController_IntegrationTest(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                services.AddAuthentication(FakeJwtBearerDefaults.AuthenticationScheme).AddFakeJwtBearer();

                services.AddAuthorization(config =>
                {
                    config.DefaultPolicy = new AuthorizationPolicyBuilder(config.DefaultPolicy)
                                                .AddAuthenticationSchemes(FakeJwtBearerDefaults.AuthenticationScheme)
                                                .RequireAuthenticatedUser()
                                                .Build();
                });
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
        response.Should().HaveStatusCode(HttpStatusCode.Forbidden);
    }

    [Theory]
    [InlineData("/api/Users/")]
    public async Task Get_Users_Should_Return_OK_With_Authentication(string url)
    {
        await GetCurrentUser(url);
    }

    private async Task<UserDTO> GetCurrentUser(string url)
    {
        // Arrange
        var client = _factory.CreateClient();
        client.SetFakeBearerToken("admin", new[] { "Admin" });
        // Act
        var response = await client.GetAsync(url);

        // Assert
        response.EnsureSuccessStatusCode();
        response.Should().HaveStatusCode(HttpStatusCode.OK);

        var responseString = await response.Content.ReadAsStringAsync();
        var responseUser = JsonConvert.DeserializeObject<List<UserDTO>>(responseString);

        _currentUser = responseUser.FirstOrDefault();

        return _currentUser;
    }

    [Theory]
    [InlineData("/api/Users/")]
    public async Task Add_Users_Should_Return_With_Authentication(string url)
    {
        // Arrange
        var client = _factory.CreateClient();
        var claims = new Dictionary<string, object>
            {
                { ClaimTypes.NameIdentifier, "test@test.com" },
            };
        client.SetFakeBearerToken(claims);

        var user = new UserDTO
        {
            Email = "email@email.com",
            Password = "password",
            FirstName = "FirstName",
            LastName = "LastName",
            Token = "token"
        };

        var myContent = JsonConvert.SerializeObject(user);
        var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
        var byteContent = new ByteArrayContent(buffer);
        byteContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

        // Act
        var response = await client.PostAsync(url, byteContent);

        // Assert
        response.EnsureSuccessStatusCode();
        response.Should().HaveStatusCode(HttpStatusCode.OK);

        var responseString = await response.Content.ReadAsStringAsync();
        var responseUser = JsonConvert.DeserializeObject<UserDTO>(responseString);

        responseUser.Should().NotBeNull();
        responseUser.Email.Should().Be(user.Email);
        responseUser.Password.Should().Be(user.Password);
        responseUser.FirstName.Should().Be(user.FirstName);
        responseUser.LastName.Should().Be(user.LastName);
        responseUser.Token.Should().Be(user.Token);
    }

    [Theory]
    [InlineData("/api/Users/")]
    public async Task Update_Users_Should_Return_With_Authentication(string url)
    {
        _currentUser = await GetCurrentUser("/api/Users/");

        // Arrange
        var client = _factory.CreateClient();
        var claims = new Dictionary<string, object>
            {
                { ClaimTypes.NameIdentifier, "test@test.com" },
            };
        client.SetFakeBearerToken(claims);

        var user = new UserDTO
        {
            Id = _currentUser.Id,
            Email = _currentUser.Email,
            Password = "password",
            FirstName = "John Update",
            LastName = "Doe",
            Token = "token"
        };

        var myContent = JsonConvert.SerializeObject(user);
        var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
        var byteContent = new ByteArrayContent(buffer);
        byteContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

        // Act
        var response = await client.PutAsync(url, byteContent);

        // Assert
        response.EnsureSuccessStatusCode();
        response.Should().HaveStatusCode(HttpStatusCode.OK);

        var responseString = await response.Content.ReadAsStringAsync();
        var responseUser = JsonConvert.DeserializeObject<UserDTO>(responseString);

        responseUser.Should().NotBeNull();
        responseUser.Email.Should().Be(user.Email);
        responseUser.Password.Should().Be(user.Password);
        responseUser.FirstName.Should().Be(user.FirstName);
        responseUser.LastName.Should().Be(user.LastName);
        responseUser.Token.Should().Be(user.Token);
    }
}