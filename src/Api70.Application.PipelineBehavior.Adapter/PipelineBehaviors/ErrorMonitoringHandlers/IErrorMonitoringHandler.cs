using System.Threading.Tasks;
using FluentResults;

namespace Api70.Application.PipelineBehavior.PipelineBehaviors.ErrorMonitoringHandlers;

public interface IErrorMonitoringHandler
{
    Task ReportAsync(Result result);
}