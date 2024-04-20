using MediatR;
using Poc.Microsservicosv2.Base.Messages;

namespace Poc.Microsservicosv2.Pagamento.Domain.Events;

public sealed class PagamentoPedidoRealizadoEvent : Event, INotification
{
    public required Guid IdPedido { get; init; }
}
