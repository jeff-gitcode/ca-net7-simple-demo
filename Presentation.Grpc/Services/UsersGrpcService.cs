using Application.Interface.API;

using Domain;

using Grpc.Core;

using Presentation.Grpc;

namespace Presentation.Grpc.Services;

public class UsersGrpcService : UsersService.UsersServiceBase
{
    private readonly ILogger<UsersGrpcService> _logger;
    private readonly IUserUseCase _userUseCase;
    public UsersGrpcService(ILogger<UsersGrpcService> logger, IUserUseCase userUseCase)
    {
        _logger = logger;
        _userUseCase = userUseCase;
    }

    public async override Task<UsersList> GetAllUsers(Empty request, ServerCallContext context)
    {

        _logger.LogInformation("GetAllUsers");

        var users = await _userUseCase.GetAllUsers();

        var response = new UsersList();

        foreach (var user in users)
        {
            response.Users.Add(new User
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Password = user.Password,
                Token = user.Token
            });
        }

        return response;
    }

    public async override Task<User> GetUserById(UserId request, ServerCallContext context)
    {
        _logger.LogInformation("GetUserById");

        var user = await _userUseCase.GetUserById(request.Id);

        return new User
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Password = user.Password,
            Token = user.Token
        };
    }

    public async override Task<User> CreateUser(User request, ServerCallContext context)
    {
        _logger.LogInformation("CreateUser");

        var user = await _userUseCase.CreateUser(new UserDTO
        {
            Id = request.Id,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            Password = request.Password,
            Token = request.Token
        });

        return new User
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Password = user.Password,
            Token = user.Token
        };
    }

    public async override Task<User> UpdateUser(User request, ServerCallContext context)
    {
        _logger.LogInformation("UpdateUser");

        var user = await _userUseCase.UpdateUser(new UserDTO
        {
            Id = request.Id,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            Password = request.Password,
            Token = request.Token
        });

        return new User
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Password = user.Password,
            Token = user.Token
        };
    }

    public async override Task<Empty> DeleteUser(UserId request, ServerCallContext context)
    {
        _logger.LogInformation("DeleteUser");

        await _userUseCase.DeleteUser(request.Id);

        return new Empty();
    }
}
