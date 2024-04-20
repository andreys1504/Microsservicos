using Poc.Microsservicosv2.Base.Infra.RabbitMq;
using Poc.Microsservicosv2.Base.Infra.RabbitMq.Consumer;
using Poc.Microsservicosv2.Base.MessageBrokerLayer;
using Poc.Microsservicosv2.Base.Settings;
using Poc.Microsservicosv2.Base.Settings.MessageBroker.DefaultConfigs;
using RabbitMQ.Client;

namespace Poc.Microsservicosv2.Pedidos.MessageBroker.IntegrationEventsContexts;

public static class ConsumersConfig
{
    public static IEnumerable<ConsumerSetup> Register(IConnection connectionRabbitMq, EnvironmentSettings environmentSettings)
    {
        IModel channelDefault = CreateChannel.Create(connectionRabbitMq);

        foreach (MappingEventContextOrigin consumer in Pedidos.Application.MessageBroker.IntegrationEventsContexts.IntegrationEventsConsumersConfig.Mappings(environmentSettings))
        {
            yield return CreateConsumerSetup.CreateConsumerSetupEvents(
                    channel: channelDefault,
                    exchangeQueueSettings: new ExchangeQueueSettings
                    {
                        Exchange = consumer.ExchangeEventOrigin,
                        Queue = consumer.ConsumerQueue,
                        RoutingKey = consumer.ConsumerRoutingKey
                    },
                    messagesPerSecond: consumer.MessagesPerSecond);
        }
    }
}
