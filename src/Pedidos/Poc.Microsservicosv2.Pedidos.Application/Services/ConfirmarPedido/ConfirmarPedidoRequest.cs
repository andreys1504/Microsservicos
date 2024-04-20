using MediatR;
using Poc.Microsservicosv2.Base.Messages;

namespace Poc.Microsservicosv2.Pedidos.Application.Services.ConfirmarPedido;

public sealed class ConfirmarPedidoRequest : Command, IRequest<bool>
{
    public required Guid IdPedido { get; init; }
}
