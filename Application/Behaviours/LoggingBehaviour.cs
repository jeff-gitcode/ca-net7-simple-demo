using MediatR;

using Microsoft.Extensions.Logging;

namespace Application.Behaviours
{

    public class LoggerPipelineBehavior<TRequest, TResponse>
        : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<LoggerPipelineBehavior<TRequest, TResponse>> logger;

        public LoggerPipelineBehavior(ILogger<LoggerPipelineBehavior<TRequest, TResponse>> logger)
        {
            this.logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            this.logger.LogInformation("Executing request: {Name} {@Request}", typeof(TRequest).Name, request);

            var response = await next();

            this.logger.LogInformation("Executing response: {Name} {@Response}", typeof(TResponse).Name, response);

            return response;
        }
    }
}