using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Poc.Microsservicosv2.Base.Infra.RabbitMq.Consumer;
using Poc.Microsservicosv2.Base.Settings;

namespace Poc.Microsservicosv2.Pagamento.MessageBroker.AsyncOperationsOnPagamento.Commands;

public sealed class ConsumerCommandsOnPagamento : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly EnvironmentSettings _environmentSettings;
    private IConsumerCommandRabbitMq _consumerCommandRabbitMq;

    public ConsumerCommandsOnPagamento(
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

        _consumerCommandRabbitMq = scope.ServiceProvider.GetRequiredService<IConsumerCommandRabbitMq>();

        await _consumerCommandRabbitMq.ConsumerCommandAsync(
            queueName: nameof(Contexts.Pagamento) + _environmentSettings.MessageBroker.DefaultConfigs.Commands.Queue,
            currentScopeConsumer: scope,
            serviceProvider: _serviceProvider,
            consumer: async (args) =>
            {
                await args.MediatorHandler.SendCommandObjectToHandlerAsync(
                    serializedCommand: args.SerializedCommandEvent,
                    commandName: args.CommandEventName_FullName);
            });
    }

    public override void Dispose()
    {
        _consumerCommandRabbitMq?.Dispose();
        base.Dispose();
    }
}