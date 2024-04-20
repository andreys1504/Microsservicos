using Microsoft.Extensions.DependencyInjection;

namespace Poc.Microsservicosv2.Base.Mediator;

public static class Dependencies
{
    public static void Register(IServiceCollection services)
    {
        services.AddTransient<IMediatorHandler, MediatorHandler>();
    }
}
