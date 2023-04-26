using System.Threading.Tasks;
using FluentResults;

namespace Api70.Application.PipelineBehaviors;

public interface IErrorMonitoringHandler
{
    Task ReportAsync(Result result);
}