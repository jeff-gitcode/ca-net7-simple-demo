using Application.SPI;
using Domain;
using FluentValidation;
using MediatR;

namespace Application.Users;

public record UpdateUserCommand(UserDTO tempUser) : IRequest<UserDTO>;

public class UpdateUserHandler : IRequestHandler<UpdateUserCommand, UserDTO>
{
    private readonly IUserRepository _userRepository;

    public UpdateUserHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserDTO> Handle(UpdateUserCommand request, CancellationToken cancellationToken) => await _userRepository.UpdateAsync(request.tempUser);
}

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(v => v.tempUser.Email)
            .NotEmpty();

        RuleFor(v => v.tempUser.Password)
            .NotEmpty();
    }
}



