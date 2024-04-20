using Poc.Microsservicosv2.Base.Infra.RabbitMq.Consumer;
using Poc.Microsservicosv2.Base.Settings;

namespace Poc.Microsservicosv2.Pedidos.Application.MessageBroker.IntegrationEventsContexts.EventsHandlers.CatalogoContext;

public sealed class CatalogoEvents_ConsumerIntegrationEvents : ConsumerIntegrationEvents
{
    public CatalogoEvents_ConsumerIntegrationEvents(
        IServiceProvider serviceProvider,
        EnvironmentSettings environmentSettings)
        : base(
            serviceProvider,
            queue: IntegrationEventsConsumersConfig.CatalogoEventsQueue,
            consumersConfigs: IntegrationEventsConsumersConfig.Mappings(Contexts.Catalogo, environmentSettings))
    {
    }
}
