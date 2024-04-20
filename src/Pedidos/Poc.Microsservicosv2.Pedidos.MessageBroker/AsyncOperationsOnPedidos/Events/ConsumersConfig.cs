using Poc.Microsservicosv2.Base.Infra.RabbitMq;
using Poc.Microsservicosv2.Base.Infra.RabbitMq.Consumer;
using Poc.Microsservicosv2.Base.Settings;
using Poc.Microsservicosv2.Base.Settings.MessageBroker.DefaultConfigs;
using RabbitMQ.Client;

namespace Poc.Microsservicosv2.Pedidos.MessageBroker.AsyncOperationsOnPedidos.Events;

public static class ConsumersConfig
{
    public static List<ConsumerSetup> Register(IConnection connectionRabbitMq, EnvironmentSettings environmentSettings)
    {
        IModel channelDefault = CreateChannel.Create(connectionRabbitMq);

        string prefixo = nameof(Contexts.Pedidos);

        return
            [
                

                //ConsumerEventsOnPedidos
                CreateConsumerSetup.CreateConsumerSetupEvents(
                    channel: channelDefault,
                    exchangeQueueSettings: new ExchangeQueueSettings
                    {
                        Exchange = prefixo + environmentSettings.MessageBroker.DefaultConfigs.Events.Exchange,
                        Queue = prefixo + environmentSettings.MessageBroker.DefaultConfigs.Events.Queue,
                        RoutingKey = environmentSettings.MessageBroker.DefaultConfigs.Events.RoutingKey
                    },
                    messagesPerSecond: environmentSettings.MessageBroker.DefaultConfigs.Events.MessagesPerSecondConsumer)
            ];
    }
}
