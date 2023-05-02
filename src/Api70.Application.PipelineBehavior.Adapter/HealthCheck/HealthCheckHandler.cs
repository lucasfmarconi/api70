using System;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Api70.Application.PipelineBehavior.Adapter.HealthCheck;
internal class HealthCheckHandler : IHealthCheckHandler
{
    private readonly ILogger<HealthCheckHandler> logger;
    private readonly HealthCheckService healthCheckService;
    private readonly IMemoryCache memoryCache;

    public HealthCheckHandler(ILogger<HealthCheckHandler> logger, HealthCheckService healthCheckService,
        IMemoryCache memoryCache)
    {
        this.logger = logger;
        this.healthCheckService = healthCheckService;
        this.memoryCache = memoryCache;
    }

    public async Task<HealthReport> PerformAndStoreHealthChecks(string tag = default,
        CancellationToken cancellationToken = default)
    {
        logger.LogTrace("Performing registered health checks. HC Tag: {tag}", tag);

        var healthReport = tag != default ?
            await healthCheckService.CheckHealthAsync(healthCheck => healthCheck.Tags.Contains(tag), cancellationToken)
            : await healthCheckService.CheckHealthAsync(cancellationToken);

        logger.LogTrace("Health check report: {@healthReport}", healthReport);

        if (healthReport.Status != HealthStatus.Healthy)
            logger.LogWarning("Application Health Status is {Status}", healthReport.Status);
        else
            logger.LogDebug("Application Health Status is {Status}", healthReport.Status);

        await memoryCache.GetOrCreateAsync("HcStatus", item =>
        {
            item.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);
            return Task.FromResult(healthReport);
        });
        
        return healthReport;
    }
}
