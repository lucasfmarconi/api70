using Api70.Application.PipelineBehaviors.ErrorMonitoringHandlers;
using FluentResults;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Api70.Application.PipelineBehaviors;
public class FailedResultMonitoringPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IErrorMonitoringHandler errorHandler;

    public FailedResultMonitoringPipelineBehavior(IErrorMonitoringHandler errorHandler)
    {
        this.errorHandler = errorHandler ?? ErrorMonitoringHandler.Instance;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var response = await next().ConfigureAwait(false);
        
        if (response is Result { IsFailed: true } result)
            await errorHandler.ReportAsync(result).ConfigureAwait(false);

        return response;
    }
}
