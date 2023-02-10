using Api70.Infrastructure.RabbitMq.Settings;
using RabbitMQ.Client;

namespace Api70.Infrastructure.RabbitMq.Connection;
internal static class RabbitMqConnectionFactory
{
    public static ConnectionFactory CreateConnectionFactory(RabbitMqSetting rabbitMqSetting, string clientProvidedName)
    {
        var factory = new ConnectionFactory
        {
            HostName = rabbitMqSetting.Host,
            DispatchConsumersAsync = true,
            Port = rabbitMqSetting.Port,
            UserName = rabbitMqSetting.UserName,
            Password = rabbitMqSetting.Password,
            VirtualHost = rabbitMqSetting.VirtualHost,
            ClientProvidedName = clientProvidedName,
            //--- TopologyRecoveryEnabled and AutomaticRecoveryEnabled properties are disabled due to these limitations
            //--- https://www.rabbitmq.com/dotnet-api-guide.html#automatic-recovery-limitations
            //--- This implements a logic and a retry policy in case of connection disruptions 
            //--- Be aware of some driver design decisions it is decided to adopt the option of removing this persistent
            //--- connection logic in order to enable the following properties
            TopologyRecoveryEnabled = false,
            AutomaticRecoveryEnabled = false
        };

        if (rabbitMqSetting.UseSsl)
        {
            factory.Ssl = new SslOption
            {
                Enabled = true
            };
        }

        return factory;
    }
}
