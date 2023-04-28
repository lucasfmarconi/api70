using FluentResults;
using System.Threading.Tasks;

namespace Api70.Application.PipelineBehaviors.ErrorMonitoringHandlers;
public class ErrorMonitoringHandler : IErrorMonitoringHandler
{
    public static IErrorMonitoringHandler Instance { get; } = new ErrorMonitoringHandler();
    public Task ReportAsync(Result result) => Task.CompletedTask;
}
