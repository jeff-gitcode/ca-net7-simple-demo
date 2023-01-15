using System.Security.Principal;

using Application.SPI;

using Domain;

using FluentValidation;

using Infrastructure.Authentication;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

public class IdentityService : IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager;

    private readonly IUserClaimsPrincipalFactory<ApplicationUser> _userClaimsPrincipalFactory;

    private readonly IAuthorizationService _authorizationService;

    public IdentityService(UserManager<ApplicationUser> userManager, IUserClaimsPrincipalFactory<ApplicationUser> userClaimsPrincipalFactory, IAuthorizationService authorizationService)
    {
        _userManager = userManager;
        _userClaimsPrincipalFactory = userClaimsPrincipalFactory;
        _authorizationService = authorizationService;
    }

    public async Task<bool> AddAsync(LoginDTO request)
    {
        try
        {
            var user = new ApplicationUser()
            {
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

    public async Task<bool> IsExistingUser(string username, string password)
    {
        var user = await _userManager.FindByNameAsync(username);
        if (user == null)
        {
            return false;
        }

        var result = await _userManager.CheckPasswordAsync(user, password);
        if (!result)
        {
            return false;
        }
        return result;
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
}