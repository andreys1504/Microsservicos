using MediatR;
using Poc.Microsservicosv2.Base.Messages;

namespace Poc.Microsservicosv2.Catalogo.Application.Services.ValidarPedidoCriado;

public sealed class ValidarPedidoCriadoRequest : Command, IRequest<bool>
{
    public required Guid IdPedido { get; init; }
    public required Guid IdProduto { get; init; }
    public required int Quantidade { get; init; }
}
