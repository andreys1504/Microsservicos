using Poc.Microsservicosv2.Base.Settings;

namespace Poc.Microsservicosv2.Base.MessageBrokerLayer;

public sealed record MappingEventContextOrigin
{
    public required Contexts ContextOrigin { get; init; }
    public required string EventKey { get; init; }
    public required string ExchangeEventOrigin { get; init; }
    public required string ConsumerQueue { get; init; }
    public required string ConsumerRoutingKey { get; init; }
    public required Type CurrentContextEvent { get; init; }
    public required ushort MessagesPerSecond { get; init; }
}
