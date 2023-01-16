using Application.Behaviours;

using Application.Interface.SPI;

using Domain;

using FluentAssertions;

using FluentValidation.TestHelper;

using MediatR;

using Microsoft.Extensions.Logging;

using Moq;

namespace UnitTests.Application.Behaviours;

public class AuthorizationBehaviourTest
{
    private readonly AuthorizationBehaviour<LoginQuery, LoginDTO> _authorizationBehaviour;
    private readonly Mock<ICurrentUserService> _currentUserService;
    private readonly Mock<IIdentityService> _identityService;

    public AuthorizationBehaviourTest()
    {
        _currentUserService = new Mock<ICurrentUserService>();
        _identityService = new Mock<IIdentityService>();

        _authorizationBehaviour = new AuthorizationBehaviour<LoginQuery, LoginDTO>(_currentUserService.Object, _identityService.Object);
    }

    [Theory]
    [InlineData("test", "test")]
    public async Task AuthorizationBehaviour_Should_ReturnNoError(string email, string password)
    {
        var request = new LoginQuery(new LoginDTO { Email = email, Password = password });

        _currentUserService.Setup(x => x.UserId).Returns("test");

        _identityService.Setup(x => x.IsInRoleAsync(_currentUserService.Object.UserId, "test")).ReturnsAsync(true);

        var result = await _authorizationBehaviour.Handle(request, () => Task.FromResult(new LoginDTO()), CancellationToken.None);

        result.Should().NotBeNull();
    }

    [Theory]
    [InlineData("test", "test")]
    public void AuthorizationBehaviour_Should_ReturnError(string email, string password)
    {
        var request = new LoginQuery(new LoginDTO { Email = email, Password = password });

        _currentUserService.Setup(x => x.UserId).Returns("test");

        _identityService.Setup(x => x.IsInRoleAsync(_currentUserService.Object.UserId, "test")).ReturnsAsync(false);

        var result = async () => await _authorizationBehaviour.Handle(request, () => Task.FromResult(new LoginDTO()), CancellationToken.None);

        result.Should().ThrowAsync<Exception>();
    }
}