using MediatR;
using Poc.Microsservicosv2.Base.Messages;

namespace Poc.Microsservicosv2.Pegamento.Application.Services.RealizarPagamento;

public sealed class RealizarPagamentoRequest : Command, IRequest<bool>
{
    public required Guid IdPedido { get; init; }
    public required decimal ValorTotal { get; init; }
}
