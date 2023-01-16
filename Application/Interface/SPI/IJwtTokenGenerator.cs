using Domain;

namespace Application.Interface.SPI
{
    public interface IJWTTokenGenerator
    {
        string CreateToken(LoginDTO user);
    }
}