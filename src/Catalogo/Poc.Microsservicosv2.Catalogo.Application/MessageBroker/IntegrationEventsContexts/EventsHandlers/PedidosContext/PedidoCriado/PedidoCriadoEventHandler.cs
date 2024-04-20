using MediatR;
using Poc.Microsservicosv2.Base.ApplicationLayer.EventsHandlers;
using Poc.Microsservicosv2.Catalogo.Application.MessageBroker.IntegrationEventsContexts.Events.PedidosContext;
using Poc.Microsservicosv2.Catalogo.Application.Services.ValidarPedidoCriado;

namespace Poc.Microsservicosv2.Catalogo.Application.MessageBroker.IntegrationEventsContexts.EventsHandlers.PedidosContext.PedidoCriado;

public sealed class PedidoCriadoEventHandler : EventHandlerBase, INotificationHandler<PedidoCriadoEvent>
{
    public PedidoCriadoEventHandler(
        DependenciesEventHandlerBase dependencies) : base(dependencies)
    {
    }

    public async Task Handle(PedidoCriadoEvent notification, CancellationToken cancellationToken)
    {
        //Consumer 'Catalogo' - Application (IntegrationEvents)

        var request = new ValidarPedidoCriadoRequest
        {
            IdPedido = notification.IdPedido,
            IdProduto = notification.IdProduto,
            Quantidade = notification.Quantidade,
        };

        await SendCommandToHandlerAsync(request);
    }
}
