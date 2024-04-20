using Poc.Microsservicosv2.Catalogo.Domain.Events;

namespace Poc.Microsservicosv2.Catalogo.Application.MessageBroker.AsyncOperationsOnCatalogo;

public static class PublishersEventsConfig
{
    public static List<(Type @event, string eventKey, string routingKey)> Register()
    {
        return
            [
                (typeof(EstoqueConfirmadoParaPedidoEvent), "EstoqueConfirmadoParaPedidoEvent", "catalogoCtx.pedidoEntity.estoqueConfirmadoParaPedidoEvent"),
                (typeof(EstoqueRejeitadoParaPedidoEvent), "EstoqueRejeitadoParaPedidoEvent", "catalogoCtx.pedidoEntity.estoqueRejeitadoParaPedidoEvent")
            ];
    }
}
