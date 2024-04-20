using MediatR;
using Poc.Microsservicosv2.Base.Messages;

namespace Poc.Microsservicosv2.Pegamento.Application.MessageBroker.IntegrationEventsContexts.Events.PedidosContext;

public sealed class StatusPedidoAlteradoParaConfirmadoEvent : Event, INotification
{
    public required Guid IdPedido { get; init; }
    public required decimal ValorTotal { get; init; }
}
