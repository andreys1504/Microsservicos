using Poc.Microsservicosv2.Base.Infra.RabbitMq.Consumer;
using Poc.Microsservicosv2.Base.Settings;

namespace Poc.Microsservicosv2.Pegamento.Application.MessageBroker.IntegrationEventsContexts.EventsHandlers.PedidosContext;

public sealed class PedidoEvents_ConsumerIntegrationEvents : ConsumerIntegrationEvents
{
    public PedidoEvents_ConsumerIntegrationEvents(
        IServiceProvider serviceProvider,
        EnvironmentSettings environmentSettings)
        : base(
            serviceProvider,
            queue: IntegrationEventsConsumersConfig.PedidoEventsQueue,
            consumersConfigs: IntegrationEventsConsumersConfig.Mappings(Contexts.Pedidos, environmentSettings))
    {
    }
}
