using Api70.Core.Messages;
using Api70.Infrastructure.RabbitMq.Connection;
using Api70.Infrastructure.RabbitMq.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System;
using System.ComponentModel.DataAnnotations;

namespace Api70.Infrastructure.RabbitMq;
public static class Module
{
    public const string SectionName = "RabbitMQ";
    public static IServiceCollection RegisterRabbitMqInfrastructure(this IServiceCollection services,
        IConfigurationSection configurationSection)
    {
        var rabbitOptions = configurationSection.Get<RabbitMqSetting>(c => c.BindNonPublicProperties = true);
        if (rabbitOptions == null)
            throw new ArgumentNullException(nameof(rabbitOptions));

        Validator.ValidateObject(rabbitOptions, new ValidationContext(rabbitOptions), validateAllProperties: true);

        services.AddSingleton<IConnectionFactory>(_ =>
            RabbitMqConnectionFactory.CreateConnectionFactory(rabbitOptions, rabbitOptions.ClientName))
            .AddHealthChecks()
            .AddRabbitMQ();

        services.AddSingleton<IRabbitMqPersistentConnection>(serviceProvider =>
        {
            var logger = serviceProvider.GetRequiredService<ILogger<RabbitMqPersistentConnection>>();
            var factory = serviceProvider.GetRequiredService<IConnectionFactory>();
            return new RabbitMqPersistentConnection(factory, logger, rabbitOptions.RetryCount);
        });

        services.AddSingleton<IBrokerMessagePublisher, BrokerMessagePublisher>();

        return services;
    }
}
