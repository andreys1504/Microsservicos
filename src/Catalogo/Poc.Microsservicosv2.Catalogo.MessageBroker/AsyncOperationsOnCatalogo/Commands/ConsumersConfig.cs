using Poc.Microsservicosv2.Base.Infra.RabbitMq;
using Poc.Microsservicosv2.Base.Infra.RabbitMq.Consumer;
using Poc.Microsservicosv2.Base.Settings;
using Poc.Microsservicosv2.Base.Settings.MessageBroker.DefaultConfigs;
using RabbitMQ.Client;

namespace Poc.Microsservicosv2.Catalogo.MessageBroker.AsyncOperationsOnCatalogo.Commands;

public static class ConsumersConfig
{
    public static List<ConsumerSetup> Register(IConnection connectionRabbitMq, EnvironmentSettings environmentSettings)
    {
        IModel channelDefault = CreateChannel.Create(connectionRabbitMq);

        string prefixo = nameof(Contexts.Catalogo);

        return
            [
                

                //Consumer Default
                CreateConsumerSetup.CreateConsumerSetupCommands(
                    channel: channelDefault,
                    exchangeQueueSettings: new ExchangeQueueSettings
                    {
                        Exchange = prefixo + environmentSettings.MessageBroker.DefaultConfigs.Commands.Exchange,
                        Queue = prefixo + environmentSettings.MessageBroker.DefaultConfigs.Commands.Queue,
                        RoutingKey = prefixo + environmentSettings.MessageBroker.DefaultConfigs.Commands.RoutingKey
                    },
                    messagesPerSecond: environmentSettings.MessageBroker.DefaultConfigs.Commands.MessagesPerSecondConsumer)
            ];
    }
}
