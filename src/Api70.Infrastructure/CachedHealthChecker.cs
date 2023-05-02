using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Api70.Infrastructure;
internal class CachedHealthChecker : IHealthCheck
{
    private readonly IMemoryCache memoryCache;
    private readonly HealthCheckService healthCheckService;


    public CachedHealthChecker(IMemoryCache memoryCache, HealthCheckService healthCheckService)
    {
        this.memoryCache = memoryCache;
        this.healthCheckService = healthCheckService;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        const string healthResultMessage = "Cached Health Report.";
        
        if (memoryCache.TryGetValue<HealthReport>("HcStatus", out var cachedHealthReport) &&
            cachedHealthReport is not null)
            return new HealthCheckResult(cachedHealthReport.Status, healthResultMessage);

        var healthReport =
            await healthCheckService.CheckHealthAsync(t => !t.Name.Contains("CachedHealthCheckStatus"),
                cancellationToken);
        
        memoryCache.Set("HcStatus", healthReport, TimeSpan.FromMinutes(5));
        return new HealthCheckResult(healthReport.Status, healthResultMessage);
    }
}
