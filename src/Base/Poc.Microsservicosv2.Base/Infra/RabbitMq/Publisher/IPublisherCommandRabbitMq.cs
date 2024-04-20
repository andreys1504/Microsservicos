using Poc.Microsservicosv2.Base.Messages.MessageInBroker.Models;

namespace Poc.Microsservicosv2.Base.Infra.RabbitMq.Publisher;

public interface IPublisherCommandRabbitMq
{
    Task PublishCommandAsync(object @command, MessageInBrokerModel messageInBroker = null);

    void Dispose();
}
