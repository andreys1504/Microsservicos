using Poc.Microsservicosv2.Base.Infra.RabbitMq;
using Poc.Microsservicosv2.Base.Infra.RabbitMq.Publisher;
using Poc.Microsservicosv2.Base.Settings;
using Poc.Microsservicosv2.Base.Settings.MessageBroker.DefaultConfigs;
using Poc.Microsservicosv2.Pedidos.Application.Services.NovoPedido;
using RabbitMQ.Client;

namespace Poc.Microsservicosv2.Pedidos.MessageBroker.AsyncOperationsOnPedidos.Commands;

public sealed class PublishersConfig
{
    public static IEnumerable<PublisherSetup> Register(IConnection connectionRabbitMq, EnvironmentSettings environmentSettings)
    {
        string prefixo = nameof(Contexts.Pedidos);

        IModel channelDefault = CreateChannel.Create(connectionRabbitMq);
        IModel channel01 = CreateChannel.Create(connectionRabbitMq);
        
        IModel selectedChannel;
        foreach (Type pub in Pedidos.Application.MessageBroker.AsyncOperationsOnPedidos.PublishersCommandsConfig.Register())
        {
            selectedChannel = channelDefault;
            if (pub == typeof(NovoPedidoRequest)) //exemplo de vinculo de canal novo a um publicador
                selectedChannel = channel01;

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
