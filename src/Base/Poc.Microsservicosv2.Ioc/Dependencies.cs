using Microsoft.Extensions.DependencyInjection;
using Poc.Microsservicosv2.Base.Settings;

namespace Poc.Microsservicosv2.Ioc;

public static class Dependencies
{
    public static void Register(this IServiceCollection services, EnvironmentSettings environmentSettings)
    {
        services.AddHttpClient();

        //Base
        // .Base.ApplicationLayer
        Base.ApplicationLayer.ApplicationsServices.Dependencies.Register(services);
        Base.ApplicationLayer.EventsHandlers.Dependencies.Register(services);
        // .Base.Infra
        Base.Infra.Data.Dependencies.Register(services, environmentSettings);
        // .Base.Mediator
        Base.Mediator.Dependencies.Register(services);
        // .Base.PresentationLayer
        Base.PresentationLayer.AspNet.ControllerBase.Dependencies.Register(services);
        
        //Infra
        // .Infra.Services
        Infra.Services.Dependencies.Register(services, environmentSettings);


        //
        services.AddSingleton(environmentSettings);
    }

    public static void RegisterDependenciesRabbitMq(this IServiceCollection services)
    {
        Infra.RabbitMq.Dependencies.Register(services);
    }
}
