using Domain;

namespace Application.SPI
{
    public interface IJWTTokenGenerator
    {
        string CreateToken(LoginDTO user);
    }
}