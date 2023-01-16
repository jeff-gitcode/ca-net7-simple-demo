using Application.Authentication;

using AutoFixture.Xunit2;

using Domain;

using MapsterMapper;

using MediatR;

using Moq;

using FluentAssertions;

namespace UnitTests.Application.Authentication
{
    public class AuthUseCaseTest
    {
        private readonly Mock<IMediator> _mediator;
        private readonly Mock<IMapper> _mapper;

        private readonly AuthUseCase _authUseCase;

        public AuthUseCaseTest()
        {
            _mediator = new Mock<IMediator>();
            _mapper = new Mock<IMapper>();

            _authUseCase = new AuthUseCase(_mediator.Object, _mapper.Object);
        }

        [Theory, AutoData]
        public void Should_Return_When_Register_With_LoginDTO(LoginDTO loginDTO, UserResponse userResponse)
        {
            _mediator.Setup(x => x.Send(It.IsAny<RegisterCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(loginDTO);
            _mediator.Setup(x => x.Publish(It.IsAny<EmailNotification>(), It.IsAny<CancellationToken>()));

            _mapper.Setup(x => x.Map<UserResponse>(It.IsAny<LoginDTO>())).Returns(userResponse);

            var result = _authUseCase.Register(loginDTO);

            result.Result.Should().Be(userResponse);
        }

        [Theory, AutoData]
        public void Should_Return_When_Login_With_LoginDTO(LoginDTO loginDTO, UserResponse userResponse)
        {
            _mediator.Setup(x => x.Send(It.IsAny<LoginQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(loginDTO);

            _mapper.Setup(x => x.Map<UserResponse>(It.IsAny<LoginDTO>())).Returns(userResponse);

            var result = _authUseCase.Login(loginDTO);

            result.Result.Should().Be(userResponse);
        }

        [Theory, AutoData]
        public void Should_Return_When_Delete_With_Username(string username)
        {
            _mediator.Setup(x => x.Send(It.IsAny<DeleteCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);

            var result = _authUseCase.Delete(username);

            result.Result.Should().Be(true);
        }
    }
}