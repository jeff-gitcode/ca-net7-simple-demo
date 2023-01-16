using Domain;

namespace Application.Interface.API
{
    public interface IAuthUseCase
    {
        Task<UserResponse> Register(LoginDTO loginDTO);
        Task<UserResponse> Login(LoginDTO loginDTO);
        Task<bool> Delete(string username);
    }
}