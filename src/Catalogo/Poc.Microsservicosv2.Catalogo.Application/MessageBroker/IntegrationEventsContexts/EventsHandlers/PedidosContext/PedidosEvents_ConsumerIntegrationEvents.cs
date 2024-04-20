using Poc.Microsservicosv2.Base.Infra.RabbitMq.Consumer;
using Poc.Microsservicosv2.Base.Settings;

namespace Poc.Microsservicosv2.Catalogo.Application.MessageBroker.IntegrationEventsContexts.EventsHandlers.PedidosContext;

public sealed class PedidosEvents_ConsumerIntegrationEvents : ConsumerIntegrationEvents
{
    public PedidosEvents_ConsumerIntegrationEvents(
        IServiceProvider serviceProvider,
        EnvironmentSettings environmentSettings)
        : base(
            serviceProvider,
            queue: IntegrationEventsConsumersConfig.PedidosEventsQueue,
            consumersConfigs: IntegrationEventsConsumersConfig.Mappings(Contexts.Pedidos, environmentSettings))
    {
    }
}
