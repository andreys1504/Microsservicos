using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Poc.Microsservicosv2.Base.Settings;

namespace Poc.Microsservicosv2.Catalogo.Ioc;

public static class PresentationLayerDependencies
{
    public static void Register(IServiceCollection services, EnvironmentSettings environmentSettings)
    {
        services.AddMediatR(config => config.RegisterServicesFromAssembly(AppDomain.CurrentDomain.Load(environmentSettings.DomainLayer)));
        services.AddMediatR(config => config.RegisterServicesFromAssembly(AppDomain.CurrentDomain.Load(environmentSettings.ApplicationLayer)));

        Microsservicosv2.Ioc.Dependencies.Register(services, environmentSettings);
    }

    public static void RegisterDependenciesRabbitMq(IServiceCollection services, EnvironmentSettings environmentSettings)
    {
        Catalogo.MessageBroker.Dependencies.Register(services, environmentSettings);
        Microsservicosv2.Ioc.Dependencies.RegisterDependenciesRabbitMq(services);
    }

    public static void UseAuthRequestsBetweenApis(WebApplication app)
    {
        Base.PresentationLayer.AspNet.Authentication.RequestsBetweenApis.Middlewares.Register(app);
    }
}
