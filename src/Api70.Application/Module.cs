using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Api70.Application.PipelineBehavior;

namespace Api70.Application;
public static class Module
{
    private static Assembly ThisAssembly => typeof(Module).GetTypeInfo().Assembly;

    public static IServiceCollection RegisterApplication(this IServiceCollection services)
    {
        services.AddMediatRPipelineBehaviorAdapter();
        services.AddMediatR(ThisAssembly);
        return services;
    }
}
