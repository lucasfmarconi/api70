using Api70.Application.PipelineBehaviors;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using FluentValidation;

namespace Api70.Application;
public static class Module
{
    private static Assembly ThisAssembly => typeof(Module).GetTypeInfo().Assembly;

    public static IServiceCollection RegisterApplication(this IServiceCollection services)
    {
        services.AddMediatR(ThisAssembly);
        RegisterPipelineBehaviors(services);
        return services;
    }

    private static void RegisterPipelineBehaviors(IServiceCollection services)
    {
        // Register the error monitor BEFORE any other IPipelineBehavior-registrations!
        services.AddSingleton(typeof(IPipelineBehavior<,>), typeof(ErrorMonitoringPipelineBehavior<,>));
        services.AddSingleton<IErrorMonitoringHandler, ConsoleErrorMonitoringHandler>();
        services.AddSingleton<IErrorMonitoringHandler, HealthChecksMonitoringHandler>();

        services.AddSingleton(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));
        services.AddValidatorsFromAssembly(ThisAssembly, ServiceLifetime.Transient);
    }
}
