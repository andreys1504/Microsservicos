using Poc.Microsservicosv2.Base.Infra.RabbitMq.Consumer;
using Poc.Microsservicosv2.Base.Settings;

namespace Poc.Microsservicosv2.Pedidos.Application.MessageBroker.IntegrationEventsContexts.EventsHandlers.PagamentoContext;

public sealed class PagamentoEvents_ConsumerIntegrationEvents : ConsumerIntegrationEvents
{
    public PagamentoEvents_ConsumerIntegrationEvents(
        IServiceProvider serviceProvider,
        EnvironmentSettings environmentSettings)
        : base(
            serviceProvider,
            queue: IntegrationEventsConsumersConfig.PagamentoEventsQueue,
            consumersConfigs: IntegrationEventsConsumersConfig.Mappings(Contexts.Pagamento, environmentSettings))
    {
    }
}
