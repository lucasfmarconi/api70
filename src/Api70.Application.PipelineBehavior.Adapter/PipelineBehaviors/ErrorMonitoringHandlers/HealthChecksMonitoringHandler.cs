using System;
using System.Threading.Tasks;
using Api70.Application.PipelineBehavior.HealthCheck;
using FluentResults;
using Microsoft.Extensions.Logging;

namespace Api70.Application.PipelineBehavior.PipelineBehaviors.ErrorMonitoringHandlers;
internal class HealthChecksMonitoringHandler : IErrorMonitoringHandler
{
    private readonly ILogger<HealthChecksMonitoringHandler> logger;
    private readonly IHealthCheckHandler healthCheckHandler;

    public HealthChecksMonitoringHandler(ILogger<HealthChecksMonitoringHandler> logger,
        IHealthCheckHandler healthCheckHandler)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.healthCheckHandler = healthCheckHandler ?? throw new ArgumentNullException(nameof(healthCheckHandler));
    }

    public Task ReportAsync(Result result)
    {
        logger.LogError("Error: {@reasons}", result.Reasons);
        healthCheckHandler.PerformAndStoreHealthChecks();
        return Task.CompletedTask;
    }
}
