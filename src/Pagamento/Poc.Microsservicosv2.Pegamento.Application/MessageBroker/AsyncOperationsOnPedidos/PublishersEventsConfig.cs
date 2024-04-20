using Poc.Microsservicosv2.Pagamento.Domain.Events;

namespace Poc.Microsservicosv2.Pegamento.Application.MessageBroker.AsyncOperationsOnPedidos;

public static class PublishersEventsConfig
{
    public static List<(Type @event, string eventKey, string routingKey)> Register()
    {
        return
            [
                (typeof(PagamentoPedidoRealizadoEvent), "PagamentoPedidoRealizadoEvent", "pagamentoCtx.pedidoEntity.pagamentoPedidoRealizadoEvent")
            ];
    }
}
