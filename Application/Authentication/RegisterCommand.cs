using Application.Interface.SPI;

using Domain;

using FluentValidation;

using MediatR;


namespace Application.Authentication;

public record RegisterCommand(LoginDTO tempUser) : IRequest<LoginDTO>;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, LoginDTO>
{
    private readonly IIdentityService _identityService;
    private readonly IJWTTokenGenerator _jwtTokenGenerator;

    public RegisterCommandHandler(IIdentityService identityService, IJWTTokenGenerator jwtTokenGenerator)
    {
        _identityService = identityService;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<LoginDTO?> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        // Check if user exists
        var user = await _identityService.GetUserAsync(request.tempUser.Email, request.tempUser.Password);
        if (user != null)
        {
            throw new Exception("User already exists");
        }

        // Create user
        var result = await _identityService.AddAsync(request.tempUser);

        // Create token
        if (result)
        {
            var token = _jwtTokenGenerator.CreateToken(request.tempUser);
            var response = new LoginDTO
            {
                Email = request.tempUser.Email,
                Password = request.tempUser.Password,
                Role = request.tempUser.Role,
                Token = token
            };

            return response;
        }
        return null;
    }
}

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(v => v.tempUser.Email)
            .NotEmpty();

        RuleFor(v => v.tempUser.Password)
            .NotEmpty();
    }
}