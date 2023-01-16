using Application.Interface.SPI;

using Domain;

using FluentValidation;

using MediatR;

namespace Application.Authentication;

public record DeleteCommand(string username) : IRequest<bool>;

public class DeleteCommandHandler : IRequestHandler<DeleteCommand, bool>
{
    private readonly IIdentityService _identityService;

    public DeleteCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<bool> Handle(DeleteCommand request, CancellationToken cancellationToken)
    {
        // Check if user exists
        var user = await _identityService.GetUserAsync(request.username);
        if (user == null)
        {
            throw new Exception("User does not exist or password is incorrect");
        }

        // Delete user
        var result = await _identityService.DeleteAsync(request.username);

        return result;
    }
}