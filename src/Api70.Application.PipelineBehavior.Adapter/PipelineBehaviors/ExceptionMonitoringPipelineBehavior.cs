using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MediatR.Pipeline;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;

namespace Api70.Application.PipelineBehavior.Adapter.PipelineBehaviors;

public class ExceptionMonitoringPipelineBehavior<TRequest, TResponse, TException> : IRequestExceptionHandler<TRequest, TResponse, TException>
    where TRequest : IRequest<TResponse>
    where TException : Exception
{
    private readonly ILogger<ExceptionMonitoringPipelineBehavior<TRequest, TResponse, TException>> logger;
    private readonly HealthCheckService healthCheckService;

    public ExceptionMonitoringPipelineBehavior(
        ILogger<ExceptionMonitoringPipelineBehavior<TRequest, TResponse, TException>> logger,
        HealthCheckService healthCheckService)
    {
        this.logger = logger;
        this.healthCheckService = healthCheckService;
    }

    public async Task Handle(TRequest request, TException exception, RequestExceptionHandlerState<TResponse> state, CancellationToken cancellationToken)
    {
        logger.LogError(exception, "Something went wrong while handling request of type {@requestType}", typeof(TRequest));
        var healthCheckResult = await healthCheckService.CheckHealthAsync(cancellationToken);
        logger.LogWarning("Application Health Status is {Status}", healthCheckResult.Status);
        state.SetHandled(default!);
    }
}
