using Microsoft.Extensions.DependencyInjection;
using Poc.Microsservicosv2.Pedidos.Application.MessageBroker.IntegrationEventsContexts.EventsHandlers.CatalogoContext;
using Poc.Microsservicosv2.Pedidos.Application.MessageBroker.IntegrationEventsContexts.EventsHandlers.PagamentoContext;

namespace Poc.Microsservicosv2.Pedidos.Application.MessageBroker.IntegrationEventsContexts;

public static class IntegrationEventsConsumersRegister
{
    public static void Register(IServiceCollection services)
    {
        CatalogoContext(services);
        PagamentoContext(services);
    }

    //
    private static void CatalogoContext(IServiceCollection services)
    {
        services.AddHostedService<CatalogoEvents_ConsumerIntegrationEvents>();
    }

    private static void PagamentoContext(IServiceCollection services)
    {
        services.AddHostedService<PagamentoEvents_ConsumerIntegrationEvents>();
    }
}
