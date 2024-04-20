using Poc.Microsservicosv2.Base.MessageBrokerLayer;
using Poc.Microsservicosv2.Base.Settings;
using Poc.Microsservicosv2.Catalogo.Application.MessageBroker.IntegrationEventsContexts.Events.PedidosContext;

namespace Poc.Microsservicosv2.Catalogo.Application.MessageBroker.IntegrationEventsContexts;

public static class IntegrationEventsConsumersConfig
{
    public static string PedidosEventsQueue = $"p:{nameof(Contexts.Pedidos)}_c:{nameof(Contexts.Catalogo)}__pedido_events_queue";

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
                    EventKey = "PedidoCriadoEvent",
                    ExchangeEventOrigin = exchangeEventOrigin,
                    ConsumerQueue = PedidosEventsQueue,
                    ConsumerRoutingKey = pedidoRoutingKey,
                    CurrentContextEvent = typeof(PedidoCriadoEvent),
                    MessagesPerSecond = 100, //TODO: configurar valor padrão
                }
            ]);

        #endregion


        return mappings;
    }
}
