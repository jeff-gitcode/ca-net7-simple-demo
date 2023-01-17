using Application.Interface.API;

using AutoFixture.Xunit2;

using Domain;

using FluentAssertions;

using Infrastructure.Authentication;

using Microsoft.AspNetCore.Mvc;

using Moq;

using Presentation.WebApi.Controllers;

namespace UnitTests.Presentation.WebApi.Controllers;

public class UsersControllerTest
{
    private readonly Mock<IUserUseCase> _userUseCase;
    private readonly UsersController _usersController;

    public UsersControllerTest()
    {
        _userUseCase = new Mock<IUserUseCase>();
        _usersController = new UsersController(_userUseCase.Object);
    }

    [Theory, AutoData]
    public async Task GetAllUsers_WithValidUser_Should_ReturnsOk(UserDTO user)
    {
        _userUseCase.Setup(x => x.GetAllUsers()).ReturnsAsync(new List<UserDTO> { user });

        var response = await _usersController.GetAllUsers();

        response.Result.Should().BeOfType<OkObjectResult>();
    }

    [Theory, AutoData]
    public async Task GetUserById_WithValidUser_Should_ReturnsOk(UserDTO user)
    {
        _userUseCase.Setup(x => x.GetUserById(user.Id)).ReturnsAsync(user);

        var response = await _usersController.GetUserById(user.Id);

        response.Result.Should().BeOfType<OkObjectResult>();
    }

    [Theory, AutoData]
    public async Task CreateUser_WithValidUser_Should_ReturnsOk(UserDTO user)
    {
        _userUseCase.Setup(x => x.CreateUser(user)).ReturnsAsync(user);

        var response = await _usersController.CreateUser(user);

        response.Result.Should().BeOfType<OkObjectResult>();
    }

    [Theory, AutoData]
    public async Task UpdateUser_WithValidUser_Should_ReturnsOk(UserDTO user)
    {
        _userUseCase.Setup(x => x.UpdateUser(user)).ReturnsAsync(user);

        var response = await _usersController.UpdateUser(user);

        response.Result.Should().BeOfType<OkObjectResult>();
    }

    [Theory, AutoData]
    public async Task DeleteUser_WithValidUser_Should_ReturnsOk(UserDTO user)
    {
        _userUseCase.Setup(x => x.DeleteUser(user.Id));

        var response = await _usersController.DeleteUser(user.Id);

        response.Should().BeOfType<NoContentResult>();
    }
}