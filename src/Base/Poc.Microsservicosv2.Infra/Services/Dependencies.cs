using Microsoft.Extensions.DependencyInjection;
using Poc.Microsservicosv2.Base.Infra.Services.Logger;
using Poc.Microsservicosv2.Base.Messages.MessageInBroker;
using Poc.Microsservicosv2.Base.Settings;
using Poc.Microsservicosv2.Infra.Services.Logger;
using Poc.Microsservicosv2.Infra.Services.Messages;

namespace Poc.Microsservicosv2.Infra.Services;

public static class Dependencies
{
    //exceto RabbitMq (.RabbitMq.Dependencies)
    public static void Register(IServiceCollection services, EnvironmentSettings environmentSettings)
    {
        //Logger
        services.AddTransient(typeof(ILoggerService<>), typeof(LoggerService<>));

        //Messages
        services.AddTransient<IMessageInBrokerService, MessageInBrokerService>();

        //ServicesInMicrosservicosv2
        ServicesInMicrosservicosv2.Dependencies.Register(services, environmentSettings);
    }
}
