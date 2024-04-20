using MediatR;
using Poc.Microsservicosv2.Base.ApplicationLayer.EventsHandlers;
using Poc.Microsservicosv2.Pegamento.Application.MessageBroker.IntegrationEventsContexts.Events.PedidosContext;
using Poc.Microsservicosv2.Pegamento.Application.Services.RealizarPagamento;

namespace Poc.Microsservicosv2.Pegamento.Application.MessageBroker.IntegrationEventsContexts.EventsHandlers.PedidosContext.StatusPedidoAlteradoParaConfirmado;

public sealed class StatusPedidoAlteradoParaConfirmadoEventHandler : EventHandlerBase, INotificationHandler<StatusPedidoAlteradoParaConfirmadoEvent>
{
    public StatusPedidoAlteradoParaConfirmadoEventHandler(DependenciesEventHandlerBase dependencies) : base(dependencies)
    {
    }

    public async Task Handle(StatusPedidoAlteradoParaConfirmadoEvent notification, CancellationToken cancellationToken)
    {
        //Consumer 'Pegamento' - Application (IntegrationEvents)

        var request = new RealizarPagamentoRequest
        {
            IdPedido = notification.IdPedido,
            ValorTotal = notification.ValorTotal
        };

        await base.SendCommandToHandlerAsync(request);
    }
}
