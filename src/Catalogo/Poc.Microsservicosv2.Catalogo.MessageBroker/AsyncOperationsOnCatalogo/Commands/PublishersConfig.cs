using Poc.Microsservicosv2.Base.Infra.RabbitMq;
using Poc.Microsservicosv2.Base.Infra.RabbitMq.Publisher;
using Poc.Microsservicosv2.Base.Settings;
using Poc.Microsservicosv2.Base.Settings.MessageBroker.DefaultConfigs;
using Poc.Microsservicosv2.Catalogo.Application.MessageBroker.AsyncOperationsOnCatalogo;
using RabbitMQ.Client;

namespace Poc.Microsservicosv2.Catalogo.MessageBroker.AsyncOperationsOnCatalogo.Commands;

public sealed class PublishersConfig
{
    public static IEnumerable<PublisherSetup> Register(IConnection connectionRabbitMq, EnvironmentSettings environmentSettings)
    {
        string prefixo = nameof(Contexts.Catalogo);

        IModel channelDefault = CreateChannel.Create(connectionRabbitMq);

        IModel selectedChannel;
        foreach (Type pub in PublishersCommandsConfig.Register())
        {
            selectedChannel = channelDefault;

            yield return CreatePublisherSetup.CreatePublisherCommands(
                            publisherChannel: selectedChannel,
                            @object: pub,
                            exchangeQueueSettings: new ExchangeQueueSettings
                            {
                                Exchange = prefixo + environmentSettings.MessageBroker.DefaultConfigs.Commands.Exchange,
                                Queue = prefixo + environmentSettings.MessageBroker.DefaultConfigs.Commands.Queue,
                                RoutingKey = prefixo + environmentSettings.MessageBroker.DefaultConfigs.Commands.RoutingKey
                            });
        }
    }
}
