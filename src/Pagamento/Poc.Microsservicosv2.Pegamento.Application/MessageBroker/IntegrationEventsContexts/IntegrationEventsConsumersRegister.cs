using Microsoft.Extensions.DependencyInjection;
using Poc.Microsservicosv2.Pegamento.Application.MessageBroker.IntegrationEventsContexts.EventsHandlers.PedidosContext;

namespace Poc.Microsservicosv2.Pegamento.Application.MessageBroker.IntegrationEventsContexts;

public static class IntegrationEventsConsumersRegister
{
    public static void Register(IServiceCollection services)
    {
        PedidosContext(services);
    }

    //
    private static void PedidosContext(IServiceCollection services)
    {
        services.AddHostedService<PedidoEvents_ConsumerIntegrationEvents>();
    }
}
