using Poc.Microsservicosv2.Base.Messages;
using Poc.Microsservicosv2.Base.Settings.MessageBroker.DefaultConfigs;
using RabbitMQ.Client;

namespace Poc.Microsservicosv2.Base.Infra.RabbitMq.Consumer;

public static class CreateConsumerSetup
{
    public static ConsumerSetup CreateConsumerSetupEvents(
        IModel channel,
        ExchangeQueueSettings exchangeQueueSettings,
        ushort messagesPerSecond)
    {
        return new ConsumerSetup(
            typeMessage: TypeMessage.Event,
            channel: channel,
            exchangeName: exchangeQueueSettings.Exchange,
            queueName: exchangeQueueSettings.Queue,
            routingKey: exchangeQueueSettings.RoutingKey,
            messagesPerSecond: messagesPerSecond);
    }

    public static ConsumerSetup CreateConsumerSetupCommands(
        IModel channel,
        ExchangeQueueSettings exchangeQueueSettings,
        ushort messagesPerSecond)
    {
        return new ConsumerSetup(
            typeMessage: TypeMessage.Command,
            channel: channel,
            exchangeName: exchangeQueueSettings.Exchange,
            queueName: exchangeQueueSettings.Queue,
            routingKey: exchangeQueueSettings.RoutingKey,
            messagesPerSecond: messagesPerSecond);
    }
}
