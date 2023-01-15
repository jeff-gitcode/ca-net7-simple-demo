using System.Security.Principal;

using Application.SPI;

using Domain;

using FluentValidation;

using Infrastructure.Authentication;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

public class IdentityService : IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager;

    private readonly IUserClaimsPrincipalFactory<ApplicationUser> _userClaimsPrincipalFactory;

    private readonly IAuthorizationService _authorizationService;

    private readonly ILogger<IdentityService> _logger;

    public IdentityService(UserManager<ApplicationUser> userManager, IUserClaimsPrincipalFactory<ApplicationUser> userClaimsPrincipalFactory, IAuthorizationService authorizationService, ILogger<IdentityService> logger)
    {
        _userManager = userManager;
        _userClaimsPrincipalFactory = userClaimsPrincipalFactory;
        _authorizationService = authorizationService;
        _logger = logger;
    }

    public async Task<bool> AddAsync(LoginDTO request)
    {
        try
        {
            var user = new ApplicationUser()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = request.Email,
                Email = request.Email,
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                throw new Exception("result.Errors");
            }

            await _userManager.AddToRoleAsync(user, request.Role);

            return true;

        }
        catch (System.Exception)
        {
            throw;
        }
    }

    public async Task<LoginDTO> GetUserAsync(string username, string? password)
    {
        var users = await _userManager.Users.ToListAsync();
        _logger.LogInformation("Users: {0}", users);

        var user = await _userManager.FindByNameAsync(username);
        if (user == null)
        {
            return null;
        }

        var isSame = await _userManager.CheckPasswordAsync(user, password);
        if (!isSame)
        {
            return null;
        }

        var userRole = await _userManager.GetRolesAsync(user);

        return new LoginDTO
        {
            Email = user.Email,
            Password = user.PasswordHash,
            Role = userRole.FirstOrDefault()
        };
    }

    public async Task<bool> IsInRoleAsync(string username, string role)
    {
        var user = await _userManager.FindByNameAsync(username);

        var userRole = await _userManager.GetRolesAsync(user);

        return userRole.Contains(role);
    }

    public async Task<bool> AuthorizeAsync(string userId, string policyName)
    {
        var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);

        if (user == null)
        {
            return false;
        }

        var principal = await _userClaimsPrincipalFactory.CreateAsync(user);

        var result = await _authorizationService.AuthorizeAsync(principal, policyName);

        return result.Succeeded;
    }

    public async Task<bool> DeleteAsync(string username)
    {
        var user = await _userManager.FindByNameAsync(username);
        if (user == null)
        {
            return false;
        }

        await _userManager.DeleteAsync(user);
        return true;
    }
}