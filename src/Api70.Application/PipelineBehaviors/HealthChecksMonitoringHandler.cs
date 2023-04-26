using FluentResults;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Api70.Application.PipelineBehaviors;
internal class HealthChecksMonitoringHandler : IErrorMonitoringHandler
{
    private readonly ILogger<HealthChecksMonitoringHandler> logger;
    private readonly HealthCheckService healthCheckService;
    
    public HealthChecksMonitoringHandler(ILogger<HealthChecksMonitoringHandler> logger, HealthCheckService healthCheckService)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.healthCheckService = healthCheckService ?? throw new ArgumentNullException(nameof(healthCheckService));
    }
    
    public async Task ReportAsync(Result result)
    {
        logger.LogError("Error: {@reasons}", result.Reasons);
        var hcResult = await healthCheckService.CheckHealthAsync();
        if (hcResult.Status != HealthStatus.Healthy)
            logger.LogWarning("Health Check problem {@resultEntries}", hcResult.Entries);
    }
}
