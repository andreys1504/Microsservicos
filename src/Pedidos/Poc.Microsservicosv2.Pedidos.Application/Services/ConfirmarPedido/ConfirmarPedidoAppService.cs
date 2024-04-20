using MediatR;
using Poc.Microsservicosv2.Base.ApplicationLayer.ApplicationsServices;
using Poc.Microsservicosv2.Pedidos.Domain.Events;

namespace Poc.Microsservicosv2.Pedidos.Application.Services.ConfirmarPedido;

public sealed class ConfirmarPedidoAppService : AppServiceBase, IRequestHandler<ConfirmarPedidoRequest, bool>
{
    public ConfirmarPedidoAppService(DependenciesAppServiceBase dependencies) : base(dependencies)
    {
    }

    public async Task<bool> Handle(ConfirmarPedidoRequest request, CancellationToken cancellationToken)
    {
        //recuperar pedido repositório

        //Confirmar pedido

        var statusPedidoAlteradoParaConfirmadoEvent = new StatusPedidoAlteradoParaConfirmadoEvent
        {
            IdPedido = request.IdPedido,
            ValorTotal = (decimal)150.00
        };

        await base.SendEventToQueueAsync(statusPedidoAlteradoParaConfirmadoEvent);

        return true;
    }
}
