using Domain;

using FluentValidation.TestHelper;

using Microsoft.Extensions.Logging;

using Moq;

namespace UnitTests.Application.Authentication;

public class EmailNotificationHandlerTest
{
    private readonly EmailNotificationHandler _emailNotificationHandler;
    private readonly Mock<ILogger<EmailNotificationHandler>> _logger;

    public EmailNotificationHandlerTest()
    {

        _logger = new Mock<ILogger<EmailNotificationHandler>>();

        _emailNotificationHandler = new EmailNotificationHandler(_logger.Object);
    }

    [Theory]
    [InlineData("test", "test")]
    public void EmailNotificationHandler_Should_ReturnNoError(string email, string password)
    {
        var request = new EmailNotification(new LoginDTO { Email = email, Password = password });


        _emailNotificationHandler.Handle(request, CancellationToken.None);

        _logger.Verify(
            x => x.Log(
                It.Is<LogLevel>(l => l == LogLevel.Information),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)));

        // _logger.Setup(x => x.LogInformation("Email sentto {Email}", request.user.Email));
        // _logger.Verify(x => x.LogInformation("Email sent to {Email}", request.user.Email), Times.Once);
    }
}