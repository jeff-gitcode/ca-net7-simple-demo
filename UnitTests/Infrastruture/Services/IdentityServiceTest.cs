
using Domain;

using FluentAssertions;

using Infrastructure.Authentication;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Moq;

namespace UnitTests.Infrastructure.Services;

public class IDentityServiceTest
{
    private readonly IdentityService _identityService;
    private readonly Mock<UserManager<ApplicationUser>> _userManager;
    private readonly Mock<IUserClaimsPrincipalFactory<ApplicationUser>> _userClaimsPrincipalFactory;
    private readonly Mock<IAuthorizationService> _authorizationService;
    private readonly Mock<ILogger<IdentityService>> _logger;

    public IDentityServiceTest()
    {
        _userManager = GetUserManagerMock<ApplicationUser>();
        _userClaimsPrincipalFactory = new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>();
        _authorizationService = new Mock<IAuthorizationService>();
        _logger = new Mock<ILogger<IdentityService>>();

        _identityService = new IdentityService(_userManager.Object, _userClaimsPrincipalFactory.Object, _authorizationService.Object, _logger.Object);
    }

    [Fact]
    public async Task AddAsync_With_ValidRequest_Should_ReturnsTrue()
    {
        // Arrange
        var request = new LoginDTO()
        {
            Email = "email",
            Password = "password",
            Role = "role"
        };

        var user = new ApplicationUser()
        {
            Id = Guid.NewGuid().ToString(),
            UserName = request.Email,
            Email = request.Email,
        };

        _userManager.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), request.Password)).ReturnsAsync(IdentityResult.Success);
        _userManager.Setup(x => x.AddToRoleAsync(It.IsAny<ApplicationUser>(), request.Role)).ReturnsAsync(IdentityResult.Success);

        // Act
        var response = await _identityService.AddAsync(request);

        // Assert
        response.Should().BeTrue();
    }

    [Fact]
    public async Task AddAsync_With_InvalidRequest_Should_ReturnsFalse()
    {
        // Arrange
        var request = new LoginDTO()
        {
            Email = "email",
            Password = "password",
            Role = "role"
        };

        var user = new ApplicationUser()
        {
            Id = Guid.NewGuid().ToString(),
            UserName = request.Email,
            Email = request.Email,
        };

        _userManager.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), request.Password)).ReturnsAsync(IdentityResult.Failed());
        _userManager.Setup(x => x.AddToRoleAsync(It.IsAny<ApplicationUser>(), request.Role)).ReturnsAsync(IdentityResult.Failed());

        // Act
        var result = async () => await _identityService.AddAsync(request);

        // Assert
        result.Should().ThrowAsync<Exception>();
    }

    [Fact(Skip = "Not implemented yet")]
    public async Task GetUserAsync_With_ValidRequest_Should_ReturnsUser()
    {
        // Arrange
        var request = new LoginDTO()
        {
            Email = "email",
            Password = "password",
            Role = "role"
        };

        var user = new ApplicationUser()
        {
            Id = Guid.NewGuid().ToString(),
            UserName = request.Email,
            Email = request.Email,
        };

        var users = new List<ApplicationUser>() { user };

        _userManager.Setup(x => x.FindByEmailAsync(request.Email)).ReturnsAsync(user);
        _userManager.Setup(x => x.CheckPasswordAsync(user, request.Password)).ReturnsAsync(true);
        _userManager.Setup(x => x.GetRolesAsync(user)).ReturnsAsync(new List<string>() { request.Role });
        _userManager.SetupGet(x => x.Users).Returns(users.AsQueryable());

        // Act
        var response = await _identityService.GetUserAsync(request.Email, request.Password);

        // Assert
        response.Should().BeEquivalentTo(user);
    }

    private Mock<UserManager<TIDentityUser>> GetUserManagerMock<TIDentityUser>() where TIDentityUser : IdentityUser
    {
        return new Mock<UserManager<TIDentityUser>>(
                new Mock<IUserStore<TIDentityUser>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<IPasswordHasher<TIDentityUser>>().Object,
                new IUserValidator<TIDentityUser>[0],
                new IPasswordValidator<TIDentityUser>[0],
                new Mock<ILookupNormalizer>().Object,
                new Mock<IdentityErrorDescriber>().Object,
                new Mock<IServiceProvider>().Object,
                new Mock<ILogger<UserManager<TIDentityUser>>>().Object);
    }
}
