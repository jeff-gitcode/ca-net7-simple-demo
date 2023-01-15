using Application.API;

using Ardalis.GuardClauses;

using Domain;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.WebApi.Controllers;

// [AllowAnonymous]
public class UsersController : ApiController
{
    private readonly IUserUseCase _userUseCase;

    public UsersController(IUserUseCase userUseCase)
    {
        Guard.Against.Null(userUseCase, nameof(userUseCase));

        _userUseCase = userUseCase;
    }

    [Authorize(Roles = UserRoles.Admin)]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDTO>>> GetAllUsers()
    {
        var users = await _userUseCase.GetAllUsers();
        return Ok(users);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UserDTO>> GetUserById(string id)
    {
        var user = await _userUseCase.GetUserById(id);
        return Ok(user);
    }

    [HttpPost]
    public async Task<ActionResult<UserDTO>> CreateUser(UserDTO tempUser)
    {
        var user = await _userUseCase.CreateUser(tempUser);
        return Ok(user);
    }

    [HttpPut]
    public async Task<ActionResult<UserDTO>> UpdateUser(UserDTO tempUser)
    {
        var user = await _userUseCase.UpdateUser(tempUser);
        return Ok(user);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteUser(string id)
    {
        await _userUseCase.DeleteUser(id);
        return NoContent();
    }
}