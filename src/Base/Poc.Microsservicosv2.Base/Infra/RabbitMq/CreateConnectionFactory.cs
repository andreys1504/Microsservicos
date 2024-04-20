using Poc.Microsservicosv2.Base.Settings;
using RabbitMQ.Client;

namespace Poc.Microsservicosv2.Base.Infra.RabbitMq;

public sealed class CreateConnectionFactory
{
    public static ConnectionFactory Create(EnvironmentSettings environmentSettings, string virtualHost = "mainVHost")
    {
        return new ConnectionFactory()
        {
            NetworkRecoveryInterval = TimeSpan.FromSeconds(environmentSettings.RabbitMq.ConnectionFactory.NetworkRecoveryIntervalSeconds),
            AutomaticRecoveryEnabled = environmentSettings.RabbitMq.ConnectionFactory.AutomaticRecoveryEnabled,
            HostName = environmentSettings.RabbitMq.ConnectionFactory.HostName,
            Port = environmentSettings.RabbitMq.ConnectionFactory.Port,
            UserName = environmentSettings.RabbitMq.ConnectionFactory.UserName,
            Password = environmentSettings.RabbitMq.ConnectionFactory.Password,
            VirtualHost = virtualHost,
            ClientProvidedName = environmentSettings.CurrentProject,
            DispatchConsumersAsync = environmentSettings.RabbitMq.ConnectionFactory.DispatchConsumersAsync,
        };
    }
}
