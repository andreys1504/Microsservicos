using MediatR;
using Poc.Microsservicosv2.Base.Messages;

namespace Poc.Microsservicosv2.Pedidos.Application.Services.NovoPedido;

public sealed class NovoPedidoRequest : Command, IRequest<bool>
{
    public Guid IdProduto { get; init; }
}
