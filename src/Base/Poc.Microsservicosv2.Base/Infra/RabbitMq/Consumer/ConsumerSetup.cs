using Poc.Microsservicosv2.Base.Messages;
using RabbitMQ.Client;

namespace Poc.Microsservicosv2.Base.Infra.RabbitMq.Consumer;

public class ConsumerSetup
{
    public ConsumerSetup(
        TypeMessage typeMessage,
        IModel channel,
        string exchangeName,
        string queueName,
        string routingKey,
        ushort messagesPerSecond)
    {
        ushort prefetchCount = 1000;

        if (messagesPerSecond > 0)
            prefetchCount = (ushort)(messagesPerSecond * 5);

        string deadLetterExchange = ConfigDeadLetterQueue(channel, typeMessage);

        var argumentsQueue = new Dictionary<string, object>()
            {
                { "x-dead-letter-exchange", deadLetterExchange }
            };

        channel.ExchangeDeclare(exchange: exchangeName, type: typeMessage == TypeMessage.Event ? ExchangeType.Topic : ExchangeType.Direct, durable: true, autoDelete: false, arguments: null);
        channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: argumentsQueue);
        channel.QueueBind(queue: queueName, exchange: exchangeName, routingKey: routingKey);

        channel.BasicQos(prefetchSize: 0, prefetchCount: prefetchCount, global: false);

        ConsumerChannel = channel;
        ExchangeName = exchangeName;
        QueueName = queueName;
        RoutingKey = routingKey;
        PrefetchCount = prefetchCount;
        MessagesPerSecond = messagesPerSecond;
    }

    public IModel ConsumerChannel { get; set; }
    public string ExchangeName { get; set; }
    public string QueueName { get; set; }
    public string RoutingKey { get; set; }
    public ushort PrefetchCount { get; set; }
    public ushort MessagesPerSecond { get; set; }

    //
    private string ConfigDeadLetterQueue(IModel channel, TypeMessage typeMessage)
    {
        string exchange = "dead_letter__events_exchange";
        string queue = "dead_letter__events_queue";

        if (typeMessage == TypeMessage.Command)
        {
            exchange = "dead_letter__commands_exchange";
            queue = "dead_letter__commands_queue";
        }

        channel.ExchangeDeclare(exchange: exchange, type: ExchangeType.Fanout);
        channel.QueueDeclare(queue: queue, durable: true, exclusive: false, autoDelete: false, arguments: null);
        channel.QueueBind(queue: queue, exchange: exchange, routingKey: "");

        return exchange;
    }
}
