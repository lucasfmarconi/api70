using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Api70.Infrastructure.RabbitMq;
internal class DummyHealthChecker : IHealthCheck
{
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken()) =>
        Task.FromResult(DateTime.Now.Second % 2 == 0
            ? HealthCheckResult.Unhealthy("Dummy Unhealthy")
            : HealthCheckResult.Unhealthy("Dummy Healthy"));
}
