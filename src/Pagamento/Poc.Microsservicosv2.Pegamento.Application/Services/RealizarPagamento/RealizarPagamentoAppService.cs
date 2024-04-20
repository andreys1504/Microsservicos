using MediatR;
using Poc.Microsservicosv2.Base.ApplicationLayer.ApplicationsServices;
using Poc.Microsservicosv2.Pagamento.Domain.Events;

namespace Poc.Microsservicosv2.Pegamento.Application.Services.RealizarPagamento;

public sealed class RealizarPagamentoAppService : AppServiceBase, IRequestHandler<RealizarPagamentoRequest, bool>
{
    public RealizarPagamentoAppService(DependenciesAppServiceBase dependencies) : base(dependencies)
    {
    }

    public async Task<bool> Handle(RealizarPagamentoRequest request, CancellationToken cancellationToken)
    {
        //Gateway de pagamento

        var pagamentoPedidoRealizadoEvent = new PagamentoPedidoRealizadoEvent
        {
            IdPedido = request.IdPedido
        };

        await base.SendEventToQueueAsync(pagamentoPedidoRealizadoEvent);

        return true;
    }
}
