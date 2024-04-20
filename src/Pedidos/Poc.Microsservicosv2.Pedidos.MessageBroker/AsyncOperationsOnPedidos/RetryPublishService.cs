using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Poc.Microsservicosv2.Base.Infra.Data;
using Poc.Microsservicosv2.Base.Infra.RabbitMq.Publisher;
using Poc.Microsservicosv2.Base.Messages.MessageInBroker;
using Poc.Microsservicosv2.Base.Messages.MessageInBroker.Models;
using Poc.Microsservicosv2.Base.Settings;
using System.Data.SqlClient;

namespace Poc.Microsservicosv2.Pedidos.MessageBroker.AsyncOperationsOnPedidos;

public sealed class RetryPublishService : BackgroundService
{
    private readonly EnvironmentSettings _environmentSettings;
    private readonly IServiceProvider _serviceProvider;
    private readonly SqlConnection _sqlConnection;

    public RetryPublishService(
        EnvironmentSettings environmentSettings,
        IServiceProvider serviceProvider)
    {
        _environmentSettings = environmentSettings;
        _serviceProvider = serviceProvider;
        _sqlConnection = ConnectionDatabase.NewConnection(environmentSettings: _environmentSettings);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Delay(4000, stoppingToken);

        while (stoppingToken.IsCancellationRequested == false)
        {
            var commands = PublishCommandsAsync(stoppingToken);
            var events = PublishEventsAsync(stoppingToken);

            await Task.WhenAll(commands, events);
        }
    }

    public override void Dispose()
    {
        _sqlConnection?.Close();
        _sqlConnection?.Dispose();
        base.Dispose();
    }

    //
    private async Task PublishCommandsAsync(CancellationToken stoppingToken)
    {
        try
        {
            using IServiceScope scope = _serviceProvider.CreateScope();

            var messageInBrokerService = scope.ServiceProvider.GetRequiredService<IMessageInBrokerService>();
            IEnumerable<MessageInBrokerModel> messages = messageInBrokerService.GetMessagesToPublish(
                isEvent: false,
                secondsDelay: _environmentSettings.RabbitMq.RetryPublishSecondsDelay,
                sqlConnection: _sqlConnection,
                sqlTransaction: null);


            if (messages?.Count() == 0)
                return;


            var publisherCommandRabbitMq = scope.ServiceProvider.GetRequiredService<IPublisherCommandRabbitMq>();

            foreach (MessageInBrokerModel message in messages)
                await publisherCommandRabbitMq.PublishCommandAsync(@command: null, messageInBroker: message);


            await Task.Delay(1000, stoppingToken);
        }
        catch (Exception)
        {
            await Task.Delay(2000, stoppingToken);
        }
    }

    private async Task PublishEventsAsync(CancellationToken stoppingToken)
    {
        try
        {
            using IServiceScope scope = _serviceProvider.CreateScope();

            var messageInBrokerService = scope.ServiceProvider.GetRequiredService<IMessageInBrokerService>();
            IEnumerable<MessageInBrokerModel> messages = messageInBrokerService.GetMessagesToPublish(
                isEvent: true,
                secondsDelay: _environmentSettings.RabbitMq.RetryPublishSecondsDelay,
                sqlConnection: _sqlConnection,
                sqlTransaction: null);


            if (messages?.Count() == 0)
                return;


            var publisherEventRabbitMq = scope.ServiceProvider.GetRequiredService<IPublisherEventRabbitMq>();

            foreach (MessageInBrokerModel message in messages)
                await publisherEventRabbitMq.PublishEventAsync(@event: null, messageInBroker: message);


            await Task.Delay(1000, stoppingToken);
        }
        catch (Exception)
        {
            await Task.Delay(2000, stoppingToken);
        }
    }
}
