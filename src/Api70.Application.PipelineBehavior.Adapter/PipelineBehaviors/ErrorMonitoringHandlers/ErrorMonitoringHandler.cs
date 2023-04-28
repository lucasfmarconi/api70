using System.Threading.Tasks;
using FluentResults;

namespace Api70.Application.PipelineBehavior.Adapter.PipelineBehaviors.ErrorMonitoringHandlers;
public class ErrorMonitoringHandler : IErrorMonitoringHandler
{
    public static IErrorMonitoringHandler Instance { get; } = new ErrorMonitoringHandler();
    public Task ReportAsync(Result result) => Task.CompletedTask;
}
