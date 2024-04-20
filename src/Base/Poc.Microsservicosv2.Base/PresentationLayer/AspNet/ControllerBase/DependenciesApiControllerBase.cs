using Poc.Microsservicosv2.Base.Mediator;

namespace Poc.Microsservicosv2.Base.PresentationLayer.AspNet.ControllerBase;

public sealed class DependenciesApiControllerBase
{
    public DependenciesApiControllerBase(
        IMediatorHandler mediatorHandler)
    {
        MediatorHandler = mediatorHandler;
    }

    public IMediatorHandler MediatorHandler { get; }
}
