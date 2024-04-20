using Poc.Microsservicosv2.Pedidos.Domain.Events;

namespace Poc.Microsservicosv2.Pedidos.Application.MessageBroker.AsyncOperationsOnPedidos;

public static class PublishersEventsConfig
{
    public static List<(Type @event, string eventKey, string routingKey)> Register()
    {
        return
            [
                (typeof(StatusPedidoAlteradoParaConfirmadoEvent), "StatusPedidoAlteradoParaConfirmadoEvent", "pedidosCtx.pedidoEntity.statusPedidoAlteradoParaConfirmadoEvent"),
                (typeof(PedidoCriadoEvent), "PedidoCriadoEvent", "pedidosCtx.pedidoEntity.pedidoCriadoEvent"),
            ];
    }
}
