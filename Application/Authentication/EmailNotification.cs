using Domain;

using MediatR;

using Microsoft.Extensions.Logging;

public record EmailNotification(LoginDTO user) : INotification;

public class EmailNotificationHandler : INotificationHandler<EmailNotification>
{
    private readonly ILogger<EmailNotificationHandler> _logger;

    public EmailNotificationHandler(ILogger<EmailNotificationHandler> logger)
    {
        _logger = logger;
    }

    public async Task Handle(EmailNotification notification, CancellationToken cancellationToken)
    {
        // Send email
        _logger.LogInformation("Email sent to {Email}", notification.user.Email);
        await Task.CompletedTask;
    }
}