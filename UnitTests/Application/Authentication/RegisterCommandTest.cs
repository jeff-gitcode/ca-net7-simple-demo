using Application.Authentication;
using Application.Interface.SPI;

using AutoFixture.Xunit2;

using Domain;

using FluentAssertions;

using Moq;

namespace UnitTests.Application.Authentication;

public class RegisterCommandHandlerTest
{
    private readonly Mock<IIdentityService> _identityService;
    private readonly Mock<IJWTTokenGenerator> _jwtTokenGenerator;
    private readonly RegisterCommandHandler _registerCommandHandler;

    public RegisterCommandHandlerTest()
    {
        _identityService = new Mock<IIdentityService>();
        _jwtTokenGenerator = new Mock<IJWTTokenGenerator>();

        _registerCommandHandler = new RegisterCommandHandler(_identityService.Object, _jwtTokenGenerator.Object);
    }

    [Theory, AutoData]
    public async Task RegisterCommandHandler_Should_Existing_User_ReturnLoginDTO(RegisterCommand request)
    {
        _identityService.Setup(x => x.GetUserAsync(request.tempUser.Email, request.tempUser.Password)).ReturnsAsync(new LoginDTO());
        _identityService.Setup(x => x.AddAsync(request.tempUser)).ReturnsAsync(true);

        _jwtTokenGenerator.Setup(x => x.CreateToken(request.tempUser)).Returns("token");

        var act = async () => await _registerCommandHandler.Handle(request, CancellationToken.None);

        act.Should().ThrowAsync<Exception>().WithMessage("User already exists");
    }

    [Theory, AutoData]
    public async Task RegisterCommandHandler_Should_ReturnLoginDTO(RegisterCommand request)
    {
        _identityService.Setup(x => x.GetUserAsync(request.tempUser.Email, request.tempUser.Password)).ReturnsAsync(null as LoginDTO);
        _identityService.Setup(x => x.AddAsync(request.tempUser)).ReturnsAsync(true);

        _jwtTokenGenerator.Setup(x => x.CreateToken(request.tempUser)).Returns(request.tempUser.Token);

        var result = await _registerCommandHandler.Handle(request, CancellationToken.None);

        result.Should().BeOfType<LoginDTO>();
        // result.Should().BeEquivalentTo(request.tempUser);
    }
}