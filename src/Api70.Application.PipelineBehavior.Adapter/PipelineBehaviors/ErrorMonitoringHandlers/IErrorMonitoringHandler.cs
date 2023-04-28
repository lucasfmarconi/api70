using System.Threading.Tasks;
using FluentResults;

namespace Api70.Application.PipelineBehavior.Adapter.PipelineBehaviors.ErrorMonitoringHandlers;

public interface IErrorMonitoringHandler
{
    Task ReportAsync(Result result);
}