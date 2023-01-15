
using Domain;

namespace Application.SPI;

public interface IIdentityService
{
    Task<bool> IsExistingUser(string username, string password);

    Task<bool> IsInRoleAsync(string userId, string role);

    Task<bool> AuthorizeAsync(string userId, string policyName);

    Task<bool> AddAsync(LoginDTO request);
}
