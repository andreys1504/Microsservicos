using Microsoft.Extensions.DependencyInjection;

namespace Poc.Microsservicosv2.Base.ApplicationLayer.EventsHandlers;

public static class Dependencies
{
    public static void Register(IServiceCollection services)
    {
        services.AddTransient<DependenciesEventHandlerBase, DependenciesEventHandlerBase>();
    }
}
