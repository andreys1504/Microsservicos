using MediatR;
using Poc.Microsservicosv2.Base.ApplicationLayer.EventsHandlers;
using Poc.Microsservicosv2.Pedidos.Application.MessageBroker.IntegrationEventsContexts.Events.CatalogoContext;
using Poc.Microsservicosv2.Pedidos.Application.Services.ConfirmarPedido;

namespace Poc.Microsservicosv2.Pedidos.Application.MessageBroker.IntegrationEventsContexts.EventsHandlers.CatalogoContext.EstoqueConfirmadoParaPedido;

public sealed class EstoqueConfirmadoParaPedidoEventHandler : EventHandlerBase, INotificationHandler<EstoqueConfirmadoParaPedidoEvent>
{
    public EstoqueConfirmadoParaPedidoEventHandler(
        DependenciesEventHandlerBase dependencies) : base(dependencies)
    {
    }

    public async Task Handle(EstoqueConfirmadoParaPedidoEvent notification, CancellationToken cancellationToken)
    {
        //Consumer 'Pedidos' - Application (IntegrationEvents)

        var request = new ConfirmarPedidoRequest
        {
            IdPedido = notification.IdPedido
        };

        await base.SendCommandToHandlerAsync(request);
    }
}