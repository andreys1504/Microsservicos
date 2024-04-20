using MediatR;
using Poc.Microsservicosv2.Base.ApplicationLayer.EventsHandlers;
using Poc.Microsservicosv2.Pedidos.Domain.Events;

namespace Poc.Microsservicosv2.Pedidos.Application.EventsHandlers.PedidoCriado;

public sealed class PedidoCriadoEventHandler : EventHandlerBase, INotificationHandler<PedidoCriadoEvent>
{
    public PedidoCriadoEventHandler(
        DependenciesEventHandlerBase dependencies) : base(dependencies)
    {
    }

    public Task Handle(PedidoCriadoEvent notification, CancellationToken cancellationToken)
    {
        //Consumer 'Pedidos' - Domain (EventsHandlers)

        return Task.CompletedTask;
    }
}
