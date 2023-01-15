using Application.SPI;
using Domain;
using MediatR;

namespace Application.Users;

public record GetUserByIdQuery(string id) : IRequest<UserDTO>;

public class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, UserDTO>
{
    private readonly IUserRepository _userRepository;
    
    public GetUserByIdHandler(IUserRepository userRepository) => _userRepository = userRepository;

    public async Task<UserDTO> Handle(GetUserByIdQuery request, CancellationToken cancellationToken) =>
        await _userRepository.GetByIdAsync(request.id);
}