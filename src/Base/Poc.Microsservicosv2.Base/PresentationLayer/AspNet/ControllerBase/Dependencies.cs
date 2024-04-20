using Microsoft.Extensions.DependencyInjection;

namespace Poc.Microsservicosv2.Base.PresentationLayer.AspNet.ControllerBase;

public static class Dependencies
{
    public static void Register(IServiceCollection services)
    {
        services.AddTransient<DependenciesApiControllerBase, DependenciesApiControllerBase>();
    }
}
