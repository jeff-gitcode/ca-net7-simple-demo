using Application.Authentication;
using Application.Interface.SPI;

using AutoFixture.Xunit2;

using Domain;

using FluentAssertions;

using Moq;

namespace UnitTests.Application.Authentication;

public class LoginQueryHandlerTest
{
    private readonly Mock<IIdentityService> _identityService;
    private readonly Mock<IJWTTokenGenerator> _jwtTokenGenerator;
    private readonly LoginQueryHandler _loginQueryHandler;

    public LoginQueryHandlerTest()
    {
        _identityService = new Mock<IIdentityService>();
        _jwtTokenGenerator = new Mock<IJWTTokenGenerator>();

        _loginQueryHandler = new LoginQueryHandler(_identityService.Object, _jwtTokenGenerator.Object);
    }

    [Theory, AutoData]
    public async Task LoginQueryHandler_Should_ReturnLoginDTO(LoginQuery request)
    {
        _identityService.Setup(x => x.GetUserAsync(request.loginDTO.Email, request.loginDTO.Password)).ReturnsAsync(request.loginDTO);
        _jwtTokenGenerator.Setup(x => x.CreateToken(request.loginDTO)).Returns(request.loginDTO.Token);

        var result = await _loginQueryHandler.Handle(request, CancellationToken.None);

        result.Should().BeOfType<LoginDTO>();
        result.Should().BeEquivalentTo(request.loginDTO);
    }
}
