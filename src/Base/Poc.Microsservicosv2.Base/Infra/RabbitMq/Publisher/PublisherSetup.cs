using Poc.Microsservicosv2.Base.Messages;
using RabbitMQ.Client;

namespace Poc.Microsservicosv2.Base.Infra.RabbitMq.Publisher;

public sealed class PublisherSetup
{
    public PublisherSetup(
        TypeMessage typeMessage,
        Type @object,
        string eventKey,
        IModel channel,
        string exchangeName,
        string routingKey,
        TimeSpan? expirationMessage = null,
        byte? priority = null
      )
    {
        if (typeMessage == TypeMessage.Command)
        {
            channel.ConfirmSelect();

            channel.ExchangeDeclare(
                exchange: exchangeName,
                type: ExchangeType.Direct,
                durable: true,
                autoDelete: false,
                arguments: null);
        }
        else
        {
            channel.ConfirmSelect();

            channel.ExchangeDeclare(
                exchange: exchangeName,
                type: ExchangeType.Topic,
                durable: true,
                autoDelete: false,
                arguments: null);
        }

        EventKey = eventKey;
        ObjectFullName = @object.FullName;
        PublishChannel = channel;
        ExchangeName = exchangeName;
        RoutingKey = routingKey;
        Priority = priority;
        ExpirationMessage = (expirationMessage == null
            ? (typeMessage == TypeMessage.Command ? TimeSpan.FromHours(2) : TimeSpan.FromHours(24))
            : expirationMessage.Value).TotalMilliseconds.ToString();
    }

    public string EventKey { get; set; }
    public string ObjectFullName { get; set; }
    public IModel PublishChannel { get; set; }
    public string ExchangeName { get; set; }
    public string RoutingKey { get; set; }
    public byte? Priority { get; set; }
    public string ExpirationMessage { get; set; }
}
