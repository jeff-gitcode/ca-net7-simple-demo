using Domain;

namespace Application.SPI
{
    public interface IAuthenticationService
    {
        Task<UserDTO> Register(UserDTO userDTO);
        Task<UserDTO> Login(LoginDTO loginDTO);
    }
}
