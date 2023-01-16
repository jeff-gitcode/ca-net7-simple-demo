using Application.Interface.API;

using Domain;

using Ardalis.GuardClauses;

using MediatR;

namespace Application.Users;

public class UserUseCase : IUserUseCase
{
    private readonly ISender _mediator;
    public UserUseCase(ISender mediator)
    {
        Guard.Against.Null(mediator, nameof(mediator));

        _mediator = mediator;
    }

    // Get All Users
    public async Task<IEnumerable<UserDTO>> GetAllUsers()
    {
        var response = await _mediator.Send(new GetAllUsersQuery());

        return response;
    }
    public async Task<UserDTO> GetUserById(string id)
    {
        var response = await _mediator.Send(new GetUserByIdQuery(id));

        return response;

    }

    public async Task<UserDTO> CreateUser(UserDTO tempUser)
    {
        var response = await _mediator.Send(new CreateUserCommand(tempUser));

        return response;
    }

    public async Task<UserDTO> UpdateUser(UserDTO tempUser)
    {
        var response = await _mediator.Send(new UpdateUserCommand(tempUser));

        return response;

    }

    public async Task<Unit> DeleteUser(string id)
    {
        return await _mediator.Send(new DeleteUserCommand(id));

        // _userRepository.Delete(tempUser);
    }
}
