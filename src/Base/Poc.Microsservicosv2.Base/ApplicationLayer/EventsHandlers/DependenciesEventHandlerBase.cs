using Poc.Microsservicosv2.Base.Mediator;

namespace Poc.Microsservicosv2.Base.ApplicationLayer.EventsHandlers;

public sealed class DependenciesEventHandlerBase
{
    public DependenciesEventHandlerBase(
        IMediatorHandler mediatorHandler)
    {
        MediatorHandler = mediatorHandler;
    }

    public IMediatorHandler MediatorHandler { get; }
}
