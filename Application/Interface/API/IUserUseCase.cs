using Domain;

using MediatR;

namespace Application.Interface.API;

public interface IUserUseCase
{
    Task<IEnumerable<UserDTO>> GetAllUsers();
    Task<UserDTO> GetUserById(string id);
    Task<UserDTO> CreateUser(UserDTO tempUser);
    Task<UserDTO> UpdateUser(UserDTO tempUser);
    Task<Unit> DeleteUser(string id);
}