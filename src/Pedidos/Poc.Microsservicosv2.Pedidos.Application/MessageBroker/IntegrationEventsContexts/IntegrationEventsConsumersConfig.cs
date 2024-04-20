using Poc.Microsservicosv2.Base.MessageBrokerLayer;
using Poc.Microsservicosv2.Base.Settings;
using Poc.Microsservicosv2.Pedidos.Application.MessageBroker.IntegrationEventsContexts.Events.CatalogoContext;
using Poc.Microsservicosv2.Pedidos.Application.MessageBroker.IntegrationEventsContexts.Events.PagamentoContext;

namespace Poc.Microsservicosv2.Pedidos.Application.MessageBroker.IntegrationEventsContexts;

public static class IntegrationEventsConsumersConfig
{
    public static string CatalogoEventsQueue = $"p:{nameof(Contexts.Catalogo)}_c:{nameof(Contexts.Pedidos)}__pedido_events_queue";
    public static string PagamentoEventsQueue = $"p:{nameof(Contexts.Pagamento)}_c:{nameof(Contexts.Pedidos)}__pedido_events_queue";

    public static List<MappingEventContextOrigin> Mappings(Contexts context, EnvironmentSettings environmentSettings)
    {
        return Mappings(environmentSettings).Where(config_ => config_.ContextOrigin == context).ToList();
    }

    public static List<MappingEventContextOrigin> Mappings(EnvironmentSettings environmentSettings)
    {
        return
            [
                ..EventsInCatalogoContext(environmentSettings),
                ..EventsInPagamentoContext(environmentSettings),
            ];
    }


    //
    private static List<MappingEventContextOrigin> EventsInCatalogoContext(EnvironmentSettings environmentSettings)
    {
        var mappings = new List<MappingEventContextOrigin>();

        var context = Contexts.Catalogo;
        string exchangeEventOrigin = context + environmentSettings.MessageBroker.DefaultConfigs.Events.Exchange;



        #region PedidoEvents

        string pedidoRoutingKey = "catalogoCtx.pedidoEntity.*";

        mappings.AddRange([
                new MappingEventContextOrigin
                {
                    ContextOrigin = context,
                    EventKey = "EstoqueConfirmadoParaPedidoEvent",
                    ExchangeEventOrigin = exchangeEventOrigin,
                    ConsumerQueue = CatalogoEventsQueue,
                    ConsumerRoutingKey = pedidoRoutingKey,
                    CurrentContextEvent = typeof(EstoqueConfirmadoParaPedidoEvent),
                    MessagesPerSecond = 100, //TODO: configurar valor padrão
                }
            ]);

        #endregion


        return mappings;
    }

    private static List<MappingEventContextOrigin> EventsInPagamentoContext(EnvironmentSettings environmentSettings)
    {
        var mappings = new List<MappingEventContextOrigin>();

        var context = Contexts.Pagamento;
        string exchangeEventOrigin = context + environmentSettings.MessageBroker.DefaultConfigs.Events.Exchange;



        #region PedidoEvents

        string pedidoRoutingKey = "pagamentoCtx.pedidoEntity.*";

        mappings.AddRange([
                new MappingEventContextOrigin
                {
                    ContextOrigin = context,
                    EventKey = "PagamentoPedidoRealizadoEvent",
                    ExchangeEventOrigin = exchangeEventOrigin,
                    ConsumerQueue = PagamentoEventsQueue,
                    ConsumerRoutingKey = pedidoRoutingKey,
                    CurrentContextEvent = typeof(PagamentoPedidoRealizadoEvent),
                    MessagesPerSecond = 100, //TODO: configurar valor padrão
                }
            ]);

        #endregion


        return mappings;
    }
}