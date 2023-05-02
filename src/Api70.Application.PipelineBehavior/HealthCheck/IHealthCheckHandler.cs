using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Api70.Application.PipelineBehavior.HealthCheck;
internal interface IHealthCheckHandler
{
    Task<HealthReport> PerformAndStoreHealthChecks(string tag = default, CancellationToken cancellationToken = default);
}
