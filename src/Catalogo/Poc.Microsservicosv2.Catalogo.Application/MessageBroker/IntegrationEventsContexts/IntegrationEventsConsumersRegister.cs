using Microsoft.Extensions.DependencyInjection;
using Poc.Microsservicosv2.Catalogo.Application.MessageBroker.IntegrationEventsContexts.EventsHandlers.PedidosContext;

namespace Poc.Microsservicosv2.Catalogo.Application.MessageBroker.IntegrationEventsContexts;

public static class IntegrationEventsConsumersRegister
{
    public static void Register(IServiceCollection services)
    {
        PedidosContext(services);
    }

    //
    private static void PedidosContext(IServiceCollection services)
    {
        services.AddHostedService<PedidosEvents_ConsumerIntegrationEvents>();
    }
}
