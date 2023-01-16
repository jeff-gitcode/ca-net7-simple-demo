using Application.Interface.API;

using Ardalis.GuardClauses;

using Domain;

using MapsterMapper;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace Application.Authentication
{
    public class AuthUseCase : IAuthUseCase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        public AuthUseCase(IMediator mediator,
                           IMapper mapper)
        {
            Guard.Against.Null(mediator, nameof(mediator));
            Guard.Against.Null(mapper, nameof(mapper));

            _mediator = mediator;
            _mapper = mapper;
        }

        public async Task<UserResponse> Register(LoginDTO loginDTO)
        {
            var command = new RegisterCommand(loginDTO);
            var result = await _mediator.Send(command);

            // Publish a email notification event
            await _mediator.Publish(new EmailNotification(result));

            var response = _mapper.Map<UserResponse>(result);

            return response;
            // return await _authenticationService.Register(userDTO);
        }

        public async Task<UserResponse> Login(LoginDTO loginDTO)
        {
            var query = new LoginQuery(loginDTO);

            var result = await _mediator.Send(query);

            var response = _mapper.Map<UserResponse>(result);

            return response;
        }

        public async Task<bool> Delete(string username)
        {
            var command = new DeleteCommand(username);

            var result = await _mediator.Send(command);

            return result;
        }
    }
}