using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Poc.Microsservicosv2.Base.MessageBrokerLayer;

namespace Poc.Microsservicosv2.Base.Infra.RabbitMq.Consumer;

public abstract class ConsumerIntegrationEvents : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private IConsumerEventRabbitMq _consumerEventRabbitMq;
    private readonly string _queue;
    private readonly IEnumerable<MappingEventContextOrigin> _consumersConfigs;

    public ConsumerIntegrationEvents(
        IServiceProvider serviceProvider,
        string queue,
        IEnumerable<MappingEventContextOrigin> consumersConfigs)
    {
        _serviceProvider = serviceProvider;
        _queue = queue;
        _consumersConfigs = consumersConfigs;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (_consumersConfigs?.Count() < 1)
            return;

        stoppingToken.ThrowIfCancellationRequested();

        using IServiceScope scope = _serviceProvider.CreateScope();

        _consumerEventRabbitMq = scope.ServiceProvider.GetRequiredService<IConsumerEventRabbitMq>();

        await _consumerEventRabbitMq.ConsumerEventAsync(
            queueName: _queue,
            currentScopeConsumer: scope,
            serviceProvider: _serviceProvider,
            consumer: async (args) =>
            {
                MappingEventContextOrigin config = _consumersConfigs.FirstOrDefault(config_ => config_.EventKey == args.EventKey);
                if (config == null)
                    return;

                await args.MediatorHandler.SendEventToHandlerAsync(
                    JsonConvert.DeserializeObject(value: args.SerializedCommandEvent, type: config.CurrentContextEvent));
            });
    }

    public override void Dispose()
    {
        _consumerEventRabbitMq?.Dispose();
        base.Dispose();
    }
}
