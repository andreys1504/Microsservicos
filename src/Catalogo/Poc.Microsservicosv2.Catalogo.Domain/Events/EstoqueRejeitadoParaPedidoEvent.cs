using MediatR;
using Poc.Microsservicosv2.Base.Messages;

namespace Poc.Microsservicosv2.Catalogo.Domain.Events;

public sealed class EstoqueRejeitadoParaPedidoEvent : Event, INotification
{
    public required Guid IdPedido { get; init; }
}
