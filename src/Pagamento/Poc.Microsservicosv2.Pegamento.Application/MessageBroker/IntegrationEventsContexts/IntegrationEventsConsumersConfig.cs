using Poc.Microsservicosv2.Base.MessageBrokerLayer;
using Poc.Microsservicosv2.Base.Settings;
using Poc.Microsservicosv2.Pegamento.Application.MessageBroker.IntegrationEventsContexts.Events.PedidosContext;

namespace Poc.Microsservicosv2.Pegamento.Application.MessageBroker.IntegrationEventsContexts;

public static class IntegrationEventsConsumersConfig
{
    public static string PedidoEventsQueue = $"p:{nameof(Contexts.Pedidos)}_c:{nameof(Contexts.Pagamento)}__pedido_events_queue";

    public static List<MappingEventContextOrigin> Mappings(Contexts context, EnvironmentSettings environmentSettings)
    {
        return Mappings(environmentSettings).Where(config_ => config_.ContextOrigin == context).ToList();
    }

    public static List<MappingEventContextOrigin> Mappings(EnvironmentSettings environmentSettings)
    {
        return
            [
                ..EventsInPedidosContext(environmentSettings)
            ];
    }


    //
    private static List<MappingEventContextOrigin> EventsInPedidosContext(EnvironmentSettings environmentSettings)
    {
        var mappings = new List<MappingEventContextOrigin>();

        var context = Contexts.Pedidos;
        string exchangeEventOrigin = context + environmentSettings.MessageBroker.DefaultConfigs.Events.Exchange;



        #region PedidoEvents

        string pedidoRoutingKey = "pedidosCtx.pedidoEntity.*";

        mappings.AddRange([
                new MappingEventContextOrigin
                {
                    ContextOrigin = context,
                    EventKey = "StatusPedidoAlteradoParaConfirmadoEvent",
                    ExchangeEventOrigin = exchangeEventOrigin,
                    ConsumerQueue = PedidoEventsQueue,
                    ConsumerRoutingKey = pedidoRoutingKey,
                    CurrentContextEvent = typeof(StatusPedidoAlteradoParaConfirmadoEvent),
                    MessagesPerSecond = 100,
                }
            ]);

        #endregion


        return mappings;
    }
}
