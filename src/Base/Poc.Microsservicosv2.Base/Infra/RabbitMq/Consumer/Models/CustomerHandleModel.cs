using Poc.Microsservicosv2.Base.Mediator;

namespace Poc.Microsservicosv2.Base.Infra.RabbitMq.Consumer.Models;

public sealed record CustomerHandleModel
{
    public required string EventKey { get; init; }
    public required string SerializedCommandEvent { get; init; }
    public required string CommandEventName { get; init; }
    public required string CommandEventName_FullName { get; init; }
    public required IMediatorHandler MediatorHandler { get; init; }
}
