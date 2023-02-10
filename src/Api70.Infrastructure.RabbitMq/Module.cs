using Api70.Core.Messages;
using Api70.Infrastructure.RabbitMq.Connection;
using Api70.Infrastructure.RabbitMq.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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
        if(rabbitOptions == null)
            throw new ArgumentNullException(nameof(rabbitOptions));

        Validator.ValidateObject(rabbitOptions, new ValidationContext(rabbitOptions), validateAllProperties: true);

        services.AddSingleton<IBrokerMessagePublisher>(serviceProvider =>
        {
            var logger = serviceProvider.GetRequiredService<ILogger<BrokerMessagePublisher>>();
            var factory = RabbitMqConnectionFactory.CreateConnectionFactory(rabbitOptions, rabbitOptions.ClientName);
            var persistentConnectionLogger =
                serviceProvider.GetRequiredService<ILogger<RabbitMqPersistentConnection>>();

            return new BrokerMessagePublisher(logger,
                new RabbitMqPersistentConnection(factory, persistentConnectionLogger, rabbitOptions.RetryCount));
        });
        return services;
    }
}
