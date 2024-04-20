using MediatR;
using Poc.Microsservicosv2.Base.ApplicationLayer.EventsHandlers;
using Poc.Microsservicosv2.Pedidos.Application.MessageBroker.IntegrationEventsContexts.Events.PagamentoContext;

namespace Poc.Microsservicosv2.Pedidos.Application.MessageBroker.IntegrationEventsContexts.EventsHandlers.PagamentoContext.PagamentoPedidoRealizado;

public sealed class PagamentoPedidoRealizadoEventHandler : EventHandlerBase, INotificationHandler<PagamentoPedidoRealizadoEvent>
{
    public PagamentoPedidoRealizadoEventHandler(DependenciesEventHandlerBase dependencies) : base(dependencies)
    {
    }

    public Task Handle(PagamentoPedidoRealizadoEvent notification, CancellationToken cancellationToken)
    {
        //TODO: implementar AppService
        return Task.CompletedTask;
    }
}
