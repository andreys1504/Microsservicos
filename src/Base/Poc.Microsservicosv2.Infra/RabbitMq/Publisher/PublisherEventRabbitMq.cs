using Newtonsoft.Json;
using Poc.Microsservicosv2.Base.Infra.Data;
using Poc.Microsservicosv2.Base.Infra.RabbitMq.Publisher;
using Poc.Microsservicosv2.Base.Infra.Services.Logger;
using Poc.Microsservicosv2.Base.Infra.Services.Logger.Models;
using Poc.Microsservicosv2.Base.Messages.MessageInBroker;
using Poc.Microsservicosv2.Base.Messages.MessageInBroker.Models;
using Poc.Microsservicosv2.Base.Settings;
using RabbitMQ.Client;
using System.Data.SqlClient;
using System.Text;

namespace Poc.Microsservicosv2.Infra.RabbitMq.Publisher;

public sealed class PublisherEventRabbitMq : IPublisherEventRabbitMq
{
    private readonly EnvironmentSettings _environmentSettings;
    private readonly SqlConnection _sqlConnection;
    private readonly IMessageInBrokerService _messageInBrokerService;
    private readonly ILoggerService<PublisherEventRabbitMq> _loggerService;
    private readonly IList<PublisherSetup> _publishersSetup;

    public PublisherEventRabbitMq(
        EnvironmentSettings environmentSettings,
        IMessageInBrokerService messageInBrokerService,
        ILoggerService<PublisherEventRabbitMq> loggerService,
        IList<PublisherSetup> publishersSetup)
    {
        _environmentSettings = environmentSettings;
        _sqlConnection = ConnectionDatabase.NewConnection(environmentSettings: _environmentSettings);
        _messageInBrokerService = messageInBrokerService;
        _loggerService = loggerService;
        _publishersSetup = publishersSetup;
    }

    public async Task PublishEventAsync(object @event, MessageInBrokerModel messageInBroker = null)
    {
        bool newPublication = false;

        string eventName_FullName = "";
        string eventName = "";
        if (@event == null)
        {
            eventName_FullName = messageInBroker.FullName;
            eventName = messageInBroker.Name;
        }
        else
        {
            eventName_FullName = @event.GetType().FullName;
            eventName = @event.GetType().Name;
            newPublication = true;
        }

        #region CreateMessage

        if (messageInBroker == null)
            try
            {
                messageInBroker = _messageInBrokerService.CreateMessage(
                    fullName: eventName_FullName,
                    name: eventName,
                    currentContext: _environmentSettings.CurrentContext,
                    body: JsonConvert.SerializeObject(@event),
                    isEvent: true,
                    originalContext: null,
                    messageIdReference: null,
                    sqlConnection: _sqlConnection,
                    sqlTransaction: null
                );
            }
            catch
            {
                return;
            }


        #endregion


        #region Configurações de Publicação

        if (_publishersSetup == null)
        {
            await _loggerService.LogErrorRegisterAsync(new LogErrorModel
            {
                Exception = new Exception(),
                Message = $"RabbitMQ; PublishEventAsync: IList<PublisherSetup> nulo",
                Keyword = "RabbitMQ"
            });
            throw new ArgumentNullException("IList<PublisherSetup> nulo");
        }

        PublisherSetup publishSetup = _publishersSetup.FirstOrDefault(publish => publish.ObjectFullName == eventName_FullName);
        if (publishSetup == null)
        {
            await _loggerService.LogErrorRegisterAsync(new LogErrorModel
            {
                Exception = new Exception(),
                Message = $"RabbitMQ; PublishEventAsync: PublisherSetup não encontrado para {eventName_FullName}",
                Keyword = "RabbitMQ"
            });
            throw new ArgumentNullException("PublisherSetup não encontrado");
        }

        #endregion


        IModel channel = publishSetup.PublishChannel;

        IBasicProperties basicProperties = channel.CreateBasicProperties();
        basicProperties.Headers = new Dictionary<string, object>
            {
                { "Content-Type", "application/json" },
                { "EventName_FullName", eventName_FullName },
                { "EventName", eventName },
                { "CurrentContext", _environmentSettings.CurrentContext },
                { "EventKey", publishSetup.EventKey }
            };
        basicProperties.DeliveryMode = 2;

        if (_environmentSettings.CurrentEnvironment != EnvironmentApp.Local)
            basicProperties.Expiration = publishSetup.ExpirationMessage;

        basicProperties.MessageId = Guid.NewGuid().ToString("D");

        using SqlTransaction sqlTransaction = _sqlConnection.BeginTransaction();
        try
        {
            if (newPublication == false)
            {
                messageInBroker = _messageInBrokerService.GetMessageByMessageId(messageId: messageInBroker.MessageId, sqlConnection: _sqlConnection, sqlTransaction: sqlTransaction);
                if (messageInBroker.MessageInBroker.HasValue)
                    return;
            }

            string postMessage = JsonConvert.SerializeObject(messageInBroker);
            byte[] body = Encoding.UTF8.GetBytes(postMessage);

            channel.BasicPublish(
                exchange: publishSetup.ExchangeName,
                routingKey: publishSetup.RoutingKey,
                mandatory: true,
                basicProperties: basicProperties,
                body: new ReadOnlyMemory<byte>(body)
            );

            if (_environmentSettings.CurrentEnvironment != EnvironmentApp.Local)
                channel.WaitForConfirmsOrDie(TimeSpan.FromSeconds(5));

            _messageInBrokerService.MarkAsMessageInBroker(messageInBroker, sqlConnection: _sqlConnection, sqlTransaction: sqlTransaction);
            sqlTransaction.Commit();
        }
        catch (Exception ex)
        {
            sqlTransaction.Rollback();
            await _loggerService.LogErrorRegisterAsync(new LogErrorModel
            {
                Exception = ex,
                Message = "RabbitMQ; PublishEventAsync: Erro ao publicar mensagem",
                Keyword = "RabbitMQ"
            });

            throw;
        }
    }


    //TODO: implementar chamada a Dispose
    public void Dispose()
    {
        _sqlConnection?.Close();
        _sqlConnection?.Dispose();
    }
}
