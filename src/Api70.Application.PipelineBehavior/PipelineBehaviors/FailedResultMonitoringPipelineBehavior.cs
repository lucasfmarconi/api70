using System.Threading;
using System.Threading.Tasks;
using Api70.Application.PipelineBehavior.PipelineBehaviors.ErrorMonitoringHandlers;
using FluentResults;
using MediatR;

namespace Api70.Application.PipelineBehavior.PipelineBehaviors;
internal class FailedResultMonitoringPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IErrorMonitoringHandler errorHandler;

    public FailedResultMonitoringPipelineBehavior(IErrorMonitoringHandler errorHandler) =>
        this.errorHandler = errorHandler ?? ErrorMonitoringHandler.Instance;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var response = await next().ConfigureAwait(false);

        if (response is Result { IsFailed: true } result)
            await errorHandler.ReportAsync(result).ConfigureAwait(false);

        return response;
    }
}
