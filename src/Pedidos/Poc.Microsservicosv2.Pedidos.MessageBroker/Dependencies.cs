using Microsoft.Extensions.DependencyInjection;
using Poc.Microsservicosv2.Base.Infra.RabbitMq;
using Poc.Microsservicosv2.Base.Infra.RabbitMq.Consumer;
using Poc.Microsservicosv2.Base.Infra.RabbitMq.Publisher;
using Poc.Microsservicosv2.Base.ResiliencePolicies;
using Poc.Microsservicosv2.Base.Settings;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;

namespace Poc.Microsservicosv2.Pedidos.MessageBroker;

public static class Dependencies
{
    public static void Register(IServiceCollection services, EnvironmentSettings environmentSettings)
    {
        ConnectionFactory connectionFactory = CreateConnectionFactory.Create(environmentSettings);
        IConnection connection = CreateConnection.Create(connectionFactory);

        var publishersSetup = new List<PublisherSetup>();
        var consumersSetup = new List<ConsumerSetup>();

        AsyncOperationsOnPedidos.Dependencies.Register(services, connection, environmentSettings, publishersSetup, consumersSetup);
        IntegrationEventsContexts.Dependencies.Register(services, connection, consumersSetup, environmentSettings);

        PipelineRegister(services, environmentSettings, connection, publishersSetup, consumersSetup);
    }

    private static void PipelineRegister(
        IServiceCollection services,
        EnvironmentSettings environmentSettings,
        IConnection connection,
        IList<PublisherSetup> publishersSetup,
        IList<ConsumerSetup> consumersSetup)
    {
        services.AddSingletonWithRetry<IConnection, BrokerUnreachableException>(
            serviceProvider => connection, 
            retryCount: environmentSettings.RabbitMq.RetryCountConnection);
        services.AddSingleton(publishersSetup);
        services.AddSingleton(consumersSetup);
    }
}
