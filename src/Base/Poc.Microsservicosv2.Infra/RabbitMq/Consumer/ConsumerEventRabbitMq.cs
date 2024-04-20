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

public class ConsumerEventRabbitMq : IConsumerEventRabbitMq
{
    private IModel _channel;
    private SqlConnection _sqlConnection;

    public async Task ConsumerEventAsync(
        string queueName,
        IServiceScope currentScopeConsumer,
        IServiceProvider serviceProvider,
        Action<CustomerHandleModel> consumer)
    {
        #region Objetos Base

        var loggerService1 = currentScopeConsumer.ServiceProvider.GetRequiredService<ILoggerService<ConsumerEventRabbitMq>>();
        var environmentSettings = currentScopeConsumer.ServiceProvider.GetRequiredService<EnvironmentSettings>();
        _sqlConnection = GetNewSqlConnection(environmentSettings);

        var consumersSetup = currentScopeConsumer.ServiceProvider.GetRequiredService<IList<ConsumerSetup>>();
        ConsumerSetup consumerSetup = consumersSetup.FirstOrDefault(consumer => consumer.QueueName == queueName);
        if (consumerSetup == null)
        {
            await loggerService1.LogErrorRegisterAsync(new LogErrorModel
            {
                Exception = new Exception(),
                Message = $"RabbitMQ; ConsumerEventAsync: ConsumerSetup não encontrado para {queueName}",
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

            var mediatorHandler = receivedScope.ServiceProvider.GetRequiredService<IMediatorHandler>();
            var messageInBrokerService = receivedScope.ServiceProvider.GetRequiredService<IMessageInBrokerService>();
            var loggerService2 = receivedScope.ServiceProvider.GetRequiredService<ILoggerService<ConsumerEventRabbitMq>>();

            string eventName = null;
            string eventName_FullName = null;
            MessageInBrokerModel messageInBroker = null;
            string serializedEvent = null;
            string eventKey = null;

            try
            {
                (eventName, eventName_FullName, messageInBroker, serializedEvent, eventKey) =
                    await GetMessageAsync(
                        eventArgs,
                        _channel,
                        sqlConnection: _sqlConnection,
                        messageInBrokerService,
                        loggerService2,
                        queueName: queueName,
                        environmentSettings);
            }
            catch
            {
                return;
            }


            if (messageInBroker == null || string.IsNullOrWhiteSpace(serializedEvent) || string.IsNullOrWhiteSpace(eventName))
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
                    EventKey = eventKey,
                    SerializedCommandEvent = serializedEvent,
                    CommandEventName = eventName,
                    CommandEventName_FullName = eventName_FullName,
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
                    Message = "RabbitMQ - ConsumerEventAsync: Erro ao consumir mensagem",
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
    private async Task<(string eventName, string eventName_FullName, MessageInBrokerModel messageInBroker, string serializedEvent, string eventKey)> GetMessageAsync(
        BasicDeliverEventArgs eventArgs,
        IModel channel,
        SqlConnection sqlConnection,
        IMessageInBrokerService messageInBrokerService,
        ILoggerService<ConsumerEventRabbitMq> loggerService,
        string queueName,
        EnvironmentSettings environmentSettings)
    {
        string eventName = null;
        string eventName_FullName = null;
        MessageInBrokerModel messageInBroker = null;
        string serializedEvent = null;
        string eventKey = null;


        using SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();
        try
        {
            ReadOnlyMemory<byte> body = eventArgs.Body;
            string postMessage = Encoding.UTF8.GetString(body.ToArray());

            eventName = GetValueInBasicProperties(eventArgs, key: "EventName");
            ArgumentNullException.ThrowIfNull(eventName, "eventName is null");

            eventName_FullName = GetValueInBasicProperties(eventArgs, key: "EventName_FullName");
            ArgumentNullException.ThrowIfNull(eventName_FullName, "eventName_FullName is null");

            eventKey = GetValueInBasicProperties(eventArgs, key: "EventKey");
            ArgumentNullException.ThrowIfNull(eventKey, "eventKey is null");


            string currentContextMessage = GetValueInBasicProperties(eventArgs, key: "CurrentContext");


            messageInBroker = JsonConvert.DeserializeObject<MessageInBrokerModel>(postMessage);
            serializedEvent = messageInBroker.Body;

            if (messageInBroker != null
                && currentContextMessage != environmentSettings.CurrentContext)
            {
                MessageInBrokerModel messageReference = messageInBrokerService.GetMessageByMessageIdReference(
                    messageIdReference: messageInBroker.MessageId,
                    queue: queueName,
                    sqlConnection: sqlConnection,
                    sqlTransaction: sqlTransaction);

                if (messageReference == null)
                    messageInBroker = messageInBrokerService.CreateMessage(
                        fullName: messageInBroker.FullName,
                        name: messageInBroker.Name,
                        currentContext: environmentSettings.CurrentContext,
                        body: serializedEvent,
                        isEvent: true,
                        originalContext: messageInBroker.CurrentContext,
                        messageIdReference: messageInBroker.MessageId,
                        queue: queueName,
                        sqlConnection: sqlConnection,
                        sqlTransaction: sqlTransaction);
                else
                    messageInBroker = messageReference;
            }

            sqlTransaction.Commit();
        }
        catch (Exception ex)
        {
            if (channel.IsOpen)
                channel.BasicReject(deliveryTag: eventArgs.DeliveryTag, requeue: false);

            sqlTransaction.Rollback();
            await loggerService.LogErrorRegisterAsync(new LogErrorModel
            {
                Exception = ex,
                Message = "RabbitMQ - ConsumerEventRabbitMq; GetMessage: Erro ao deserializar mensagem",
                Keyword = "RabbitMQ"
            });

            throw;
        }


        return (eventName, eventName_FullName, messageInBroker, serializedEvent, eventKey);
    }

    private string GetValueInBasicProperties(BasicDeliverEventArgs eventArgs, string key)
    {
        KeyValuePair<string, object> header = eventArgs.BasicProperties.Headers.FirstOrDefault(header_ => header_.Key == key);
        if (header.Equals(default(KeyValuePair<string, object>)))
            return null;

        return Encoding.UTF8.GetString(header.Value as byte[]);
    }

    private SqlConnection GetNewSqlConnection(EnvironmentSettings environmentSettings)
    {
        return ConnectionDatabase.NewConnection(environmentSettings: environmentSettings);
    }
}
