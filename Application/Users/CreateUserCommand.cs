using Application.SPI;
using Domain;
using FluentValidation;
using MediatR;


namespace Application.Users;

public record CreateUserCommand(UserDTO tempUser) : IRequest<UserDTO>;


public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserDTO>
{
    private readonly IUserRepository _userRepository;

    public CreateUserCommandHandler(IUserRepository userRepository) => _userRepository = userRepository;

    public async Task<UserDTO> Handle(CreateUserCommand request, CancellationToken cancellationToken) => await _userRepository.AddAsync(request.tempUser);
}

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(v => v.tempUser.Email)
            .NotEmpty();

        RuleFor(v => v.tempUser.Password)
            .NotEmpty();
    }
}



