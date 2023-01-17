using Domain;

namespace Application.Interface.API
{
    public interface IAuthUseCase
    {
        Task<UserResponse> RegisterAsync(LoginDTO loginDTO);
        Task<UserResponse> LoginAsync(LoginDTO loginDTO);
        Task<bool> DeleteAsync(string username);
    }
}
