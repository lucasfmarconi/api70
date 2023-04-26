using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Api70.Application;
public abstract class SafeHandler<TRequest> : IRequestHandler<TRequest, Result>
    where TRequest : IRequest<Result>
{
    private static readonly Type RequestType = typeof(TRequest);
    private readonly ILogger logger;

    protected SafeHandler(ILogger logger) => this.logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<Result> Handle(TRequest request, CancellationToken cancellationToken)
    {
        using (logger.BeginScope(GetType().Name))
        {
            try
            {
                logger.LogTrace("Handling request {RequestType}", RequestType);
                var result = await HandleAsync(request, cancellationToken);

                if (result.IsFailed)
                    logger.LogWarning("Command handling failed with reasons: {@reasons}", result.Reasons);

                return result;
            }
            catch (Exception ex)
            {
                logger.LogDebug(ex, "Exception when handling request of type {RequestType}", RequestType);
                return Result.Fail(new Error($"Exception when handling request of type {RequestType}").CausedBy(ex));
            }
        }
    }

    protected abstract Task<Result> HandleAsync(TRequest request, CancellationToken cancellationToken);
}
