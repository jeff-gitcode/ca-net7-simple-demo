using Domain;

namespace Application.API
{
    public interface IAuthUseCase
    {
        Task<UserResponse> Register(LoginDTO loginDTO);
        Task<UserResponse> Login(LoginDTO loginDTO);
    }
}
