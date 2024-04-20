using Microsoft.Extensions.DependencyInjection;
using Poc.Microsservicosv2.Base.Infra.RabbitMq.Consumer;
using Poc.Microsservicosv2.Base.Infra.RabbitMq.Publisher;
using Poc.Microsservicosv2.Infra.RabbitMq.Consumer;
using Poc.Microsservicosv2.Infra.RabbitMq.Publisher;

namespace Poc.Microsservicosv2.Infra.RabbitMq;

public static class Dependencies
{
    public static void Register(this IServiceCollection services)
    {
        RegisterConsumersPublishs(services);

        //TODO: Dispose para Singletons: ConnectionsRabbitMq, por exemplo
    }


    //
    private static void RegisterConsumersPublishs(IServiceCollection services)
    {
        //Consumer
        services.AddTransient<IConsumerCommandRabbitMq, ConsumerCommandRabbitMq>();
        services.AddTransient<IConsumerEventRabbitMq, ConsumerEventRabbitMq>();

        //Publisher
        services.AddTransient<IPublisherEventRabbitMq, PublisherEventRabbitMq>();
        services.AddTransient<IPublisherCommandRabbitMq, PublisherCommandRabbitMq>();
    }
}
