using Microsoft.Extensions.DependencyInjection;
using Poc.Microsservicosv2.Base.Infra.RabbitMq.Consumer.Models;

namespace Poc.Microsservicosv2.Base.Infra.RabbitMq.Consumer;

public interface IConsumerEventRabbitMq
{
    Task ConsumerEventAsync(string queueName, IServiceScope currentScopeConsumer, IServiceProvider serviceProvider, Action<CustomerHandleModel> consumer);

    void Dispose();
}
