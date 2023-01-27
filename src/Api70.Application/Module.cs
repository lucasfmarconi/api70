using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Api70.Application;
public static class Module
{
    public static IServiceCollection RegisterApplication(this IServiceCollection services)
    {
        services.AddMediatR(typeof(Module));
        return services;
    }
}
