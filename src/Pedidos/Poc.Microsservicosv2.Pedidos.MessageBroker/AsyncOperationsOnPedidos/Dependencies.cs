using Microsoft.Extensions.DependencyInjection;
using Poc.Microsservicosv2.Base.Infra.RabbitMq.Consumer;
using Poc.Microsservicosv2.Base.Infra.RabbitMq.Publisher;
using Poc.Microsservicosv2.Base.Settings;
using Poc.Microsservicosv2.Pedidos.MessageBroker.AsyncOperationsOnPedidos.Commands;
using Poc.Microsservicosv2.Pedidos.MessageBroker.AsyncOperationsOnPedidos.Events;
using RabbitMQ.Client;

namespace Poc.Microsservicosv2.Pedidos.MessageBroker.AsyncOperationsOnPedidos;

public static class Dependencies
{
    public static void Register(
        IServiceCollection services,
        IConnection connection,
        EnvironmentSettings environmentSettings,
        List<PublisherSetup> publishersSetup,
        List<ConsumerSetup> consumersSetup)
    {
        PublishersAndConsumersConfigRegister(connection, environmentSettings, publishersSetup, consumersSetup);
        ConsumersRegister(services);
    }

    //
    private static void PublishersAndConsumersConfigRegister(
        IConnection connection,
        EnvironmentSettings environmentSettings,
        List<PublisherSetup> publishersSetup,
        List<ConsumerSetup> consumersSetup)
    {
        publishersSetup.AddRange(Commands.PublishersConfig.Register(connection, environmentSettings));
        consumersSetup.AddRange(Commands.ConsumersConfig.Register(connection, environmentSettings));

        publishersSetup.AddRange(Events.PublishersConfig.Register(connection, environmentSettings));
        consumersSetup.AddRange(Events.ConsumersConfig.Register(connection, environmentSettings));
    }

    private static void ConsumersRegister(IServiceCollection services)
    {
        services.AddHostedService<RetryPublishService>();

        //ConsumersCommands
        services.AddHostedService<ConsumerCommandsOnPedidos>();

        //ConsumersEvents
        services.AddHostedService<ConsumerEventsOnPedidos>();
    }
}
