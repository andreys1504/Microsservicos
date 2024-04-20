using Poc.Microsservicosv2.Base.Messages;

namespace Poc.Microsservicosv2.Base.ApplicationLayer.ApplicationsServices;

public abstract class AppServiceBase
{
    private readonly DependenciesAppServiceBase _dependencies;

    public AppServiceBase(
        DependenciesAppServiceBase dependencies)
    {
        _dependencies = dependencies;
    }

    protected async Task SendEventToQueueAsync(Event @event)
    {
        await _dependencies.MediatorHandler.SendEventToQueueAsync(@event);
    }
}
