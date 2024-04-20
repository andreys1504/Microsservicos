using Poc.Microsservicosv2.Base.Mediator;

namespace Poc.Microsservicosv2.Base.ApplicationLayer.ApplicationsServices;

public sealed class DependenciesAppServiceBase
{
    public DependenciesAppServiceBase(
        IMediatorHandler mediatorHandler)
    {
        MediatorHandler = mediatorHandler;
    }

    public IMediatorHandler MediatorHandler { get; }
}
