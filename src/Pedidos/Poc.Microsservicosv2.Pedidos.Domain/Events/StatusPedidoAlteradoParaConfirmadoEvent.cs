using MediatR;
using Poc.Microsservicosv2.Base.Messages;

namespace Poc.Microsservicosv2.Pedidos.Domain.Events;

public sealed class StatusPedidoAlteradoParaConfirmadoEvent : Event, INotification
{
    public required Guid IdPedido { get; init; }
    public required decimal ValorTotal { get; init; }
}
