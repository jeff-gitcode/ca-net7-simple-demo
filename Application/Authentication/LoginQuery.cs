using Application.SPI;

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
        var isExisting = await _identityService.IsExistingUser(request.loginDTO.Email, request.loginDTO.Password);
        if (!isExisting)
        {
            throw new Exception("User does not exist or password is incorrect");
        }

        // Create token
        var token = _jwtTokenGenerator.CreateToken(request.loginDTO);
        var response = new LoginDTO
        {
            Email = request.loginDTO.Email,
            Password = request.loginDTO.Password,
            Token = token
        };

        return response;
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