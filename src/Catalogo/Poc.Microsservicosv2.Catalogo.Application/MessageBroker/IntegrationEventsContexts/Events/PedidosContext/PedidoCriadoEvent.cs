using MediatR;
using Poc.Microsservicosv2.Base.Messages;

namespace Poc.Microsservicosv2.Catalogo.Application.MessageBroker.IntegrationEventsContexts.Events.PedidosContext;

public sealed class PedidoCriadoEvent : Event, INotification
{
    public required Guid IdPedido { get; init; }
    public required DateTime DataHoraRealizacaoPedido { get; init; }
    public required Guid IdProduto { get; init; }
    public required int Quantidade { get; init; }
}
