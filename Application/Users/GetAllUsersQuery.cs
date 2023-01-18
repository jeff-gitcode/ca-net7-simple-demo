using Application.Interface.SPI;

using Domain;

using MediatR;

namespace Application.Users;

public record GetAllUsersQuery : IRequest<IEnumerable<UserDTO>>;

public class GetAllUsersHandler : IRequestHandler<GetAllUsersQuery, IEnumerable<UserDTO>>
{
    private readonly IUserRepository _userRepository;
    private readonly ICacheUsersService _cacheService;

    public GetAllUsersHandler(IUserRepository userRepository, ICacheUsersService cacheService)
    {
        _userRepository = userRepository;
        _cacheService = cacheService;
    }

    public async Task<IEnumerable<UserDTO>> Handle(GetAllUsersQuery request,
        CancellationToken cancellationToken)
    {
        var cacheKey = "users";
        var users = await _cacheService.Get(cacheKey);

        if (users != null)
        {
            return users;
        }

        users = await _userRepository.GetAllAsync() ?? new List<UserDTO>();

        await _cacheService.Set(cacheKey, users);

        return users;
    }
}
