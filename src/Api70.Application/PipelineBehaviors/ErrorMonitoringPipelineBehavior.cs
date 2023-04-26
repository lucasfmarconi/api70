using FluentResults;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Api70.Application.PipelineBehaviors;
public class ErrorMonitoringPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IErrorMonitoringHandler errorHandler;

    public ErrorMonitoringPipelineBehavior(IErrorMonitoringHandler errorHandler)
    {
        this.errorHandler = errorHandler ?? ErrorMonitoringHandler.Instance;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        try
        {
            var response = await next().ConfigureAwait(false);
            if (response is Result { IsFailed: true } result)
                await errorHandler.ReportAsync(result).ConfigureAwait(false);

            return response;
        }
        catch (Exception ex)
        {
            await errorHandler.ReportAsync(Result.Fail(new Error("Unhandled exception caught by pipeline").CausedBy(ex))).ConfigureAwait(false);
            return default;
        }
    }
}
