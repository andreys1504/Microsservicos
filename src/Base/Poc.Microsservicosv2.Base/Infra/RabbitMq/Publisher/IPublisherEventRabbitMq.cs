using Poc.Microsservicosv2.Base.Messages.MessageInBroker.Models;

namespace Poc.Microsservicosv2.Base.Infra.RabbitMq.Publisher;

public interface IPublisherEventRabbitMq
{
    Task PublishEventAsync(object @event, MessageInBrokerModel messageInBroker = null);

    void Dispose();
}
