using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Poc.Microsservicosv2.Base.Infra.RabbitMq.Consumer;
using Poc.Microsservicosv2.Base.Settings;

namespace Poc.Microsservicosv2.Pagamento.MessageBroker.AsyncOperationsOnPagamento.Events;

public sealed class ConsumerEventsOnPagamento : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly EnvironmentSettings _environmentSettings;
    private IConsumerEventRabbitMq _consumerEventRabbitMq;

    public ConsumerEventsOnPagamento(
        IServiceProvider serviceProvider,
        EnvironmentSettings environmentSettings)
    {
        _serviceProvider = serviceProvider;
        _environmentSettings = environmentSettings;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();

        using IServiceScope scope = _serviceProvider.CreateScope();

        _consumerEventRabbitMq = scope.ServiceProvider.GetRequiredService<IConsumerEventRabbitMq>();

        await _consumerEventRabbitMq.ConsumerEventAsync(
            queueName: nameof(Contexts.Pagamento) + _environmentSettings.MessageBroker.DefaultConfigs.Events.Queue,
            currentScopeConsumer: scope,
            serviceProvider: _serviceProvider,
            consumer: async (args) =>
            {
                await args.MediatorHandler.SendEventObjectToHandlerAsync(
                    serializedEvent: args.SerializedCommandEvent,
                    eventName: args.CommandEventName_FullName);
            });
    }

    public override void Dispose()
    {
        _consumerEventRabbitMq?.Dispose();
        base.Dispose();
    }
}