using Poc.Microsservicosv2.Base.Settings;

namespace Poc.Microsservicosv2.Pagamento.Api;

public static class Dependencies
{
    public static void Register(
        this IServiceCollection services,
        EnvironmentSettings environmentSettings)
    {
        Ioc.PresentationLayerDependencies.Register(services, environmentSettings);

        Ioc.PresentationLayerDependencies.RegisterDependenciesRabbitMq(services, environmentSettings);
    }
}
