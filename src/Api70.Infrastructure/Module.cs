using Api70.Core.Messages;
using Api70.Infrastructure.Messages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Api70.Infrastructure;
public static class Module
{
    public static IServiceCollection RegisterInfrastructure(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddHealthChecks().AddTypeActivatedCheck<DummyHealthChecker>("Dummy HC");
        services.AddHealthChecks().AddTypeActivatedCheck<CachedHealthChecker>("CachedHealthCheckStatus");
        services.AddSingleton<IMessagePublisher, MessagePublisher>();
        return services;
    }
}
