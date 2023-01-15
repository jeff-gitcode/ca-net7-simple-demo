using Application.API;

using Ardalis.GuardClauses;

using Domain;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.WebApi.Controllers;

[AllowAnonymous] // Don't apply authorization to this controller
public class AuthenticationController : ApiController
{
    private readonly IAuthUseCase _authUseCase;

    public AuthenticationController(IAuthUseCase authUseCase)
    {
        Guard.Against.Null(authUseCase, nameof(authUseCase));

        _authUseCase = authUseCase;
    }

    [HttpPost("register")]
    public async Task<ActionResult> Register([FromBody] LoginDTO loginDTO)
    {
        // var user = _mapper.Map<TempUser>(userDTO);
        // var result = await _identityService.RegisterAsync(user);
        // if (!result.Succeeded)
        // {
        //     return BadRequest(result.Errors);
        // }
        var user = await _authUseCase.Register(loginDTO);
        return Ok(user);
    }

    [HttpPost("login")]
    public async Task<ActionResult> Login([FromBody] LoginDTO loginDTO)
    {
        // var user = _mapper.Map<TempUser>(userDTO);
        // var result = await _identityService.LoginAsync(user);
        // if (!result.Succeeded)
        // {
        //     return BadRequest(result.Errors);
        // }
        // var token = _tokenService.GenerateToken(user);
        var user = await _authUseCase.Login(loginDTO);
        return Ok(user);
    }

    [HttpDelete("delete")]
    public async Task<ActionResult> Delete([FromBody] LoginDTO loginDTO)
    {
        var deleted = await _authUseCase.Delete(loginDTO.Email);
        return Ok(deleted);
    }

}