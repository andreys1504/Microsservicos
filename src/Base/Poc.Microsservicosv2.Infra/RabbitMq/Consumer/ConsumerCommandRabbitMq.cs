using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Poc.Microsservicosv2.Base.Infra.Data;
using Poc.Microsservicosv2.Base.Infra.RabbitMq;
using Poc.Microsservicosv2.Base.Infra.RabbitMq.Consumer;
using Poc.Microsservicosv2.Base.Infra.RabbitMq.Consumer.Models;
using Poc.Microsservicosv2.Base.Infra.Services.Logger;
using Poc.Microsservicosv2.Base.Infra.Services.Logger.Models;
using Poc.Microsservicosv2.Base.Mediator;
using Poc.Microsservicosv2.Base.Messages.MessageInBroker;
using Poc.Microsservicosv2.Base.Messages.MessageInBroker.Models;
using Poc.Microsservicosv2.Base.Settings;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Data.SqlClient;
using System.Text;

namespace Poc.Microsservicosv2.Infra.RabbitMq.Consumer;

public class ConsumerCommandRabbitMq : IConsumerCommandRabbitMq
{
    private IModel _channel;
    private SqlConnection _sqlConnection;

    public async Task ConsumerCommandAsync(
        string queueName,
        IServiceScope currentScopeConsumer,
        IServiceProvider serviceProvider,
        Action<CustomerHandleModel> consumer)
    {
        #region Objetos Base

        var loggerService1 = currentScopeConsumer.ServiceProvider.GetRequiredService<ILoggerService<ConsumerCommandRabbitMq>>();
        var environmentSettings = currentScopeConsumer.ServiceProvider.GetRequiredService<EnvironmentSettings>();
        _sqlConnection = GetNewSqlConnection(environmentSettings);

        var consumersSetup = currentScopeConsumer.ServiceProvider.GetRequiredService<IList<ConsumerSetup>>();
        ConsumerSetup consumerSetup = consumersSetup.FirstOrDefault(consumer => consumer.QueueName == queueName);
        if (consumerSetup == null)
        {
            await loggerService1.LogErrorRegisterAsync(new LogErrorModel
            {
                Exception = new Exception(),
                Message = $"RabbitMQ; ConsumerCommandAsync: ConsumerSetup não encontrado para {queueName}",
                Keyword = "RabbitMQ"
            });
            return;
        }

        #endregion

        _channel = consumerSetup.ConsumerChannel;

        var eventingBasicConsumer = new AsyncEventingBasicConsumer(_channel);
        eventingBasicConsumer.Received += async (sender, eventArgs) =>
        {
            if (consumerSetup.MessagesPerSecond != 0)
                consumerSetup.MessagesPerSecond.AsMessageRateToSleepTimeSpan().Wait();


            using IServiceScope receivedScope = serviceProvider.CreateScope();

            var messageInBrokerService = receivedScope.ServiceProvider.GetRequiredService<IMessageInBrokerService>();
            var loggerService2 = receivedScope.ServiceProvider.GetRequiredService<ILoggerService<ConsumerCommandRabbitMq>>();
            var mediatorHandler = receivedScope.ServiceProvider.GetRequiredService<IMediatorHandler>();

            string commandName = null;
            string commandName_FullName = null;
            MessageInBrokerModel messageInBroker = null;
            string serializedCommand = null;

            try
            {
                (commandName, commandName_FullName, messageInBroker, serializedCommand) = await GetMessageAsync(eventArgs, _channel, loggerService2);
            }
            catch
            {
                return;
            }


            if (messageInBroker == null || string.IsNullOrWhiteSpace(serializedCommand) || string.IsNullOrWhiteSpace(commandName))
                return;

            if (messageInBroker.Processed.HasValue)
            {
                _channel.BasicAck(deliveryTag: eventArgs.DeliveryTag, multiple: false);
                return;
            }


            using SqlTransaction sqlTransaction = _sqlConnection.BeginTransaction();
            try
            {
                messageInBrokerService.MarkAsProcessed(message: messageInBroker, sqlConnection: _sqlConnection, sqlTransaction: sqlTransaction);
                consumer(new CustomerHandleModel
                {
                    EventKey = null,
                    SerializedCommandEvent = serializedCommand,
                    CommandEventName = commandName,
                    CommandEventName_FullName = commandName_FullName,
                    MediatorHandler = mediatorHandler
                });
                _channel.BasicAck(deliveryTag: eventArgs.DeliveryTag, multiple: false);

                sqlTransaction.Commit();
            }
            catch (Exception ex)
            {
                if (_channel.IsOpen)
                    _channel.BasicNack(deliveryTag: eventArgs.DeliveryTag, multiple: false, requeue: eventArgs.Redelivered == false);

                messageInBrokerService.IncrementNum(message: messageInBroker, sqlConnection: _sqlConnection, sqlTransaction: sqlTransaction);
                sqlTransaction.Commit();
                await loggerService2.LogErrorRegisterAsync(new LogErrorModel
                {
                    Exception = ex,
                    Message = "RabbitMQ - ConsumerCommandAsync: Erro ao consumir mensagem",
                    Keyword = "RabbitMQ"
                });
            }
        };

        _channel.BasicConsume(queue: queueName, autoAck: false, consumer: eventingBasicConsumer);
    }

    public void Dispose()
    {
        if (_channel != null)
        {
            if (_channel.IsOpen)
                _channel.Close();

            _channel.Dispose();
        }

        _sqlConnection?.Close();
        _sqlConnection?.Dispose();
    }


    //
    private SqlConnection GetNewSqlConnection(EnvironmentSettings environmentSettings)
    {
        return ConnectionDatabase.NewConnection(environmentSettings: environmentSettings);
    }

    private async Task<(string commandName, string commandName_FullName, MessageInBrokerModel messageInBroker, string serializedCommand)> GetMessageAsync(
        BasicDeliverEventArgs eventArgs,
        IModel channel,
        ILoggerService<ConsumerCommandRabbitMq> loggerService)
    {
        string commandName;
        string commandName_FullName;
        MessageInBrokerModel messageInBroker;
        string serializedCommand;

        try
        {
            ReadOnlyMemory<byte> body = eventArgs.Body;
            string postMessage = Encoding.UTF8.GetString(body.ToArray());


            commandName = GetValueInBasicProperties(eventArgs, key: "CommandName");
            ArgumentNullException.ThrowIfNull(commandName, "commandName is null");

            commandName_FullName = GetValueInBasicProperties(eventArgs, key: "CommandName_FullName");
            ArgumentNullException.ThrowIfNull(commandName_FullName, "commandName_FullName is null");


            messageInBroker = JsonConvert.DeserializeObject<MessageInBrokerModel>(postMessage);
            serializedCommand = messageInBroker.Body;
        }
        catch (Exception ex)
        {
            if (channel.IsOpen)
                channel.BasicReject(deliveryTag: eventArgs.DeliveryTag, requeue: false);

            await loggerService.LogErrorRegisterAsync(new LogErrorModel
            {
                Exception = ex,
                Message = "RabbitMQ - ConsumerCommandRabbitMq; GetMessage: Erro ao deserializar mensagem",
                Keyword = "RabbitMQ"
            });

            throw;
        }


        return (commandName, commandName_FullName, messageInBroker, serializedCommand);
    }

    private string GetValueInBasicProperties(BasicDeliverEventArgs eventArgs, string key)
    {
        KeyValuePair<string, object> header = eventArgs.BasicProperties.Headers.FirstOrDefault(header_ => header_.Key == key);
        if (header.Equals(default(KeyValuePair<string, object>)))
            return null;

        return Encoding.UTF8.GetString(header.Value as byte[]);
    }
}
