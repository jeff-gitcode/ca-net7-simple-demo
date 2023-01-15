
using Domain;

namespace Application.SPI;

public interface IIdentityService
{
    Task<LoginDTO> GetUserAsync(string username, string? password = null);

    Task<bool> IsInRoleAsync(string userId, string role);

    Task<bool> AuthorizeAsync(string userId, string policyName);

    Task<bool> AddAsync(LoginDTO request);
    Task<bool> DeleteAsync(string username);
}
