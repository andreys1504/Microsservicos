using Poc.Microsservicosv2.Base.Messages;
using Poc.Microsservicosv2.Base.Settings.MessageBroker.DefaultConfigs;
using RabbitMQ.Client;

namespace Poc.Microsservicosv2.Base.Infra.RabbitMq.Publisher;

public static class CreatePublisherSetup
{
    public static PublisherSetup CreatePublisherCommands(
        IModel publisherChannel,
        Type @object,
        ExchangeQueueSettings exchangeQueueSettings)
    {
        return new PublisherSetup(
            typeMessage: TypeMessage.Command,
            @object: @object,
            eventKey: null,
            channel: publisherChannel,
            exchangeName: exchangeQueueSettings.Exchange,
            routingKey: exchangeQueueSettings.RoutingKey);
    }

    public static PublisherSetup CreatePublisherEvents(
        IModel publisherChannel,
        Type @object,
        string eventKey,
        ExchangeQueueSettings exchangeQueueSettings)
    {
        return new PublisherSetup(
            typeMessage: TypeMessage.Event,
            @object: @object,
            eventKey: eventKey,
            channel: publisherChannel,
            exchangeName: exchangeQueueSettings.Exchange,
            routingKey: exchangeQueueSettings.RoutingKey);
    }
}
