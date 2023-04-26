using FluentResults;
using System.Threading.Tasks;

namespace Api70.Application.PipelineBehaviors;

public interface IErrorMonitoringHandler
{
    Task ReportAsync(Result result);
}