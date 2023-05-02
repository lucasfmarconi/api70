using System;
using System.Threading;
using System.Threading.Tasks;
using Api70.Application.PipelineBehavior.HealthCheck;
using MediatR;
using MediatR.Pipeline;
using Microsoft.Extensions.Logging;

namespace Api70.Application.PipelineBehavior.PipelineBehaviors;

internal class
    ExceptionMonitoringPipelineBehavior<TRequest, TResponse, TException> : IRequestExceptionHandler<TRequest, TResponse,
        TException>
    where TRequest : IRequest<TResponse>
    where TException : Exception
{
    private readonly ILogger<ExceptionMonitoringPipelineBehavior<TRequest, TResponse, TException>> logger;
    private readonly IHealthCheckHandler healthCheckHandler;

    public ExceptionMonitoringPipelineBehavior(
        ILogger<ExceptionMonitoringPipelineBehavior<TRequest, TResponse, TException>> logger,
        IHealthCheckHandler healthCheckHandler)
    {
        this.logger = logger;
        this.healthCheckHandler = healthCheckHandler;
    }

    public Task Handle(TRequest request, TException exception, RequestExceptionHandlerState<TResponse> state,
        CancellationToken cancellationToken)
    {
        logger.LogError(exception, "Something went wrong while handling request of type {@requestType}",
            typeof(TRequest));
       
        healthCheckHandler.PerformAndStoreHealthChecks(cancellationToken: cancellationToken);

        state.SetHandled(default!);

        return Task.CompletedTask;
    }
}
