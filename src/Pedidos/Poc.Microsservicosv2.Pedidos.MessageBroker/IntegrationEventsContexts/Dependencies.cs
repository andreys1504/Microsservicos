using Microsoft.Extensions.DependencyInjection;
using Poc.Microsservicosv2.Base.Infra.RabbitMq.Consumer;
using Poc.Microsservicosv2.Base.Settings;
using RabbitMQ.Client;

namespace Poc.Microsservicosv2.Pedidos.MessageBroker.IntegrationEventsContexts;

public static class Dependencies
{
    public static void Register(
        IServiceCollection services,
        IConnection connection,
        List<ConsumerSetup> consumersSetup,
        EnvironmentSettings environmentSettings)
    {
        ConsumersConfigRegister(connection, consumersSetup, environmentSettings);
        ConsumersRegister(services);
    }

    //
    private static void ConsumersConfigRegister(IConnection connection, List<ConsumerSetup> consumersSetup, EnvironmentSettings environmentSettings)
    {
        consumersSetup.AddRange(ConsumersConfig.Register(connection, environmentSettings));
    }

    private static void ConsumersRegister(IServiceCollection services)
    {
        Pedidos.Application.MessageBroker.IntegrationEventsContexts.IntegrationEventsConsumersRegister.Register(services);
    }
}
