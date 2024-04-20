using MediatR;
using Poc.Microsservicosv2.Base.Messages;

namespace Poc.Microsservicosv2.Pedidos.Application.MessageBroker.IntegrationEventsContexts.Events.PagamentoContext;

public sealed class PagamentoPedidoRealizadoEvent : Event, INotification
{
    public required Guid IdPedido { get; init; }
}
