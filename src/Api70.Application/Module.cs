using Api70.Application.PipelineBehaviors;
using Api70.Application.PipelineBehaviors.ErrorMonitoringHandlers;
using MediatR;
using MediatR.Pipeline;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Api70.Application;
public static class Module
{
    private static Assembly ThisAssembly => typeof(Module).GetTypeInfo().Assembly;

    public static IServiceCollection RegisterApplication(this IServiceCollection services)
    {
        RegisterPipelineBehaviors(services);
        services.AddMediatR(ThisAssembly);
        return services;
    }

    private static void RegisterPipelineBehaviors(IServiceCollection services)
    {
        // Register the error monitor BEFORE any other IPipelineBehavior-registrations!
        services.AddSingleton(typeof(IPipelineBehavior<,>), typeof(FailedResultMonitoringPipelineBehavior<,>));
        services.AddTransient(typeof(IRequestExceptionHandler<,,>), typeof(ExceptionMonitoringPipelineBehavior<,,>));

        services.AddSingleton<IErrorMonitoringHandler, ConsoleErrorMonitoringHandler>();
        services.AddSingleton<IErrorMonitoringHandler, HealthChecksMonitoringHandler>();
    }
}
