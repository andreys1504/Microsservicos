namespace Poc.Microsservicosv2.Base.Settings.MessageBroker.DefaultConfigs;

public sealed record ExchangeQueueSettings
{
    public string Exchange { get; set; }
    public string Queue { get; set; }
    public string RoutingKey { get; set; }
    public ushort MessagesPerSecondPublisher { get; set; }
    public ushort MessagesPerSecondConsumer { get; set; }
}
