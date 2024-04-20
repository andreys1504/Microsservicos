using Poc.Microsservicosv2.Base.Messages;

namespace Poc.Microsservicosv2.Base.PresentationLayer.AspNet.ControllerBase;

public abstract class ApiControllerBase : Microsoft.AspNetCore.Mvc.ControllerBase
{
    private readonly DependenciesApiControllerBase _dependencies;

    public ApiControllerBase(DependenciesApiControllerBase dependencies)
    {
        _dependencies = dependencies;
    }

    protected async Task SendCommandToQueueAsync(Command @request)
    {
        await _dependencies.MediatorHandler.SendCommandToQueueAsync(@request);
    }

    protected async Task SendCommandToHandlerAsync(object @request)
    {
        await _dependencies.MediatorHandler.SendCommandToHandlerAsync(@request);
    }
}
