using System.Threading;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Threading.Tasks;

namespace Api70.Application.PipelineBehavior.Adapter.HealthCheck;
internal interface IHealthCheckHandler
{
    Task<HealthReport> PerformAndStoreHealthChecks(string tag = default, CancellationToken cancellationToken = default);
}
