
using Domain;

namespace Application.Interface.SPI
{
    public interface ICacheUsersService
    {
        Task Set(string key, IEnumerable<UserDTO> value);
        Task<IEnumerable<UserDTO>> Get(string key);
        Task Remove(string key);
    }
}