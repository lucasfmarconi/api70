using Api70.Application.PipelineBehavior.Adapter.PipelineBehaviors;
using Api70.Application.PipelineBehavior.Adapter.PipelineBehaviors.ErrorMonitoringHandlers;
using MediatR;
using MediatR.Pipeline;
using Microsoft.Extensions.DependencyInjection;

namespace Api70.Application.PipelineBehavior.Adapter;
public static class Module
{
    public static IServiceCollection AddMediatRPipelineBehaviorAdapter(this IServiceCollection services)
    {
        RegisterPipelineBehaviors(services);
        return services;
    }

    private static void RegisterPipelineBehaviors(IServiceCollection services)
    {
        services.AddSingleton(typeof(IPipelineBehavior<,>), typeof(FailedResultMonitoringPipelineBehavior<,>));
        services.AddTransient(typeof(IRequestExceptionHandler<,,>), typeof(ExceptionMonitoringPipelineBehavior<,,>));

        services.AddSingleton<IErrorMonitoringHandler, ConsoleErrorMonitoringHandler>();
        services.AddSingleton<IErrorMonitoringHandler, HealthChecksMonitoringHandler>();
    }
}
