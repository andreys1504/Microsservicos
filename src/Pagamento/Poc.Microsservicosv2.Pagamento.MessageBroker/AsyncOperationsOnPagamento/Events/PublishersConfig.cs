using Poc.Microsservicosv2.Base.Infra.RabbitMq;
using Poc.Microsservicosv2.Base.Infra.RabbitMq.Publisher;
using Poc.Microsservicosv2.Base.Settings;
using Poc.Microsservicosv2.Base.Settings.MessageBroker.DefaultConfigs;
using Poc.Microsservicosv2.Pegamento.Application.MessageBroker.AsyncOperationsOnPedidos;
using RabbitMQ.Client;

namespace Poc.Microsservicosv2.Pagamento.MessageBroker.AsyncOperationsOnPagamento.Events;

public sealed class PublishersConfig
{
    public static IEnumerable<PublisherSetup> Register(IConnection connectionRabbitMq, EnvironmentSettings environmentSettings)
    {
        string prefixo = nameof(Contexts.Pagamento);

        IModel channelDefault = CreateChannel.Create(connectionRabbitMq);

        IModel selectedChannel;
        foreach (var publishConfig in PublishersEventsConfig.Register())
        {
            selectedChannel = channelDefault;

            yield return CreatePublisherSetup.CreatePublisherEvents(
                            publisherChannel: selectedChannel,
                            @object: publishConfig.@event,
                            eventKey: publishConfig.eventKey,
                            exchangeQueueSettings: new ExchangeQueueSettings
                            {
                                Exchange = prefixo + environmentSettings.MessageBroker.DefaultConfigs.Events.Exchange,
                                RoutingKey = publishConfig.routingKey
                            });
        }
    }
}
