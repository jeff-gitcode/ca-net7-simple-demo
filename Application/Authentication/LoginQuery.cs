using Application.Interface.SPI;

using Domain;

using FluentValidation;

using MediatR;

public record LoginQuery(LoginDTO loginDTO) : IRequest<LoginDTO>;

public class LoginQueryHandler : IRequestHandler<LoginQuery, LoginDTO>
{
    private readonly IIdentityService _identityService;
    private readonly IJWTTokenGenerator _jwtTokenGenerator;

    public LoginQueryHandler(IIdentityService identityService, IJWTTokenGenerator jwtTokenGenerator)
    {
        _identityService = identityService;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<LoginDTO> Handle(LoginQuery request, CancellationToken cancellationToken)
    {
        // Check if user exists
        var user = await _identityService.GetUserAsync(request.loginDTO.Email, request.loginDTO.Password);
        if (user == null)
        {
            throw new Exception("User does not exist or password is incorrect");
        }

        // Create token
        var token = _jwtTokenGenerator.CreateToken(user);
        user.Token = token;

        return user;
    }
}

public class LoginQueryValidator : AbstractValidator<LoginQuery>
{
    public LoginQueryValidator()
    {
        RuleFor(v => v.loginDTO.Email)
            .NotEmpty();

        RuleFor(v => v.loginDTO.Password)
            .NotEmpty();
    }
}