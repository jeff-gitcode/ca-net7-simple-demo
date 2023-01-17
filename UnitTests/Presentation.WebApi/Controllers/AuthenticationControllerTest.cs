using Application.Interface.API;

using AutoFixture.Xunit2;

using Domain;

using FluentAssertions;

using Infrastructure.Authentication;

using Microsoft.AspNetCore.Mvc;

using Moq;

using Presentation.WebApi.Controllers;

namespace UnitTests.Presentation.WebApi.Controllers;

public class AuthenticationControllerTest
{
    private readonly Mock<IAuthUseCase> _authUseCase;
    private readonly AuthenticationController _authenticationController;

    public AuthenticationControllerTest()
    {
        _authUseCase = new Mock<IAuthUseCase>();
        _authenticationController = new AuthenticationController(_authUseCase.Object);
    }

    [Theory, AutoData]
    public async Task Register_WithValidLoginDTO_Should_ReturnsOk(UserResponse user)
    {
        var loginDTO = new LoginDTO
        {
            Email = "email",
            Password = "password",
            Role = "role"
        };

        _authUseCase.Setup(x => x.RegisterAsync(loginDTO)).ReturnsAsync(user);

        var response = await _authenticationController.Register(loginDTO);

        response.Should().BeOfType<OkObjectResult>();
    }

    [Theory, AutoData]
    public async Task Login_WithValidLoginDTO_Should_ReturnsOk(UserResponse user)
    {
        var loginDTO = new LoginDTO
        {
            Email = "email",
            Password = "password",
            Role = "role"
        };

        _authUseCase.Setup(x => x.LoginAsync(loginDTO)).ReturnsAsync(user);

        var response = await _authenticationController.Login(loginDTO);

        response.Should().BeOfType<OkObjectResult>();
    }
}
