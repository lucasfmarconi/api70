using FluentResults;
using System.Threading.Tasks;

namespace Api70.Application.PipelineBehaviors.ErrorMonitoringHandlers;

public interface IErrorMonitoringHandler
{
    Task ReportAsync(Result result);
}