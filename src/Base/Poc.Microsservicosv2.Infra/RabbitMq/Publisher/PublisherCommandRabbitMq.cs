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

public sealed class PublisherCommandRabbitMq : IPublisherCommandRabbitMq
{
    private readonly EnvironmentSettings _environmentSettings;
    private readonly SqlConnection _sqlConnection;
    private readonly IMessageInBrokerService _messageInBrokerService;
    private readonly ILoggerService<PublisherCommandRabbitMq> _loggerService;
    private readonly IList<PublisherSetup> _publishersSetup;

    public PublisherCommandRabbitMq(
        EnvironmentSettings environmentSettings,
        IMessageInBrokerService messageInBrokerService,
        ILoggerService<PublisherCommandRabbitMq> loggerService,
        IList<PublisherSetup> publishersSetup)
    {
        _environmentSettings = environmentSettings;
        _sqlConnection = ConnectionDatabase.NewConnection(environmentSettings: _environmentSettings);
        _messageInBrokerService = messageInBrokerService;
        _loggerService = loggerService;
        _publishersSetup = publishersSetup;
    }


    public async Task PublishCommandAsync(object @command, MessageInBrokerModel messageInBroker = null)
    {
        bool newPublication = false;

        string commandName_FullName = "";
        string commandName = "";
        if (@command == null)
        {
            commandName_FullName = messageInBroker.FullName;
            commandName = messageInBroker.Name;
        }
        else
        {
            commandName_FullName = @command.GetType().FullName;
            commandName = @command.GetType().Name;
            newPublication = true;
        }

        #region CreateMessage

        if (messageInBroker == null)
            try
            {
                messageInBroker = _messageInBrokerService.CreateMessage(
                    fullName: commandName_FullName,
                    name: commandName,
                    currentContext: _environmentSettings.CurrentContext,
                    body: JsonConvert.SerializeObject(@command),
                    isEvent: false,
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
                Message = $"RabbitMQ; PublishCommandAsync: IList<PublisherSetup> nulo",
                Keyword = "RabbitMQ"
            });
            throw new ArgumentNullException("IList<PublisherSetup> nulo");
        }

        PublisherSetup publishSetup = _publishersSetup.FirstOrDefault(publish => publish.ObjectFullName == commandName_FullName);
        if (publishSetup == null)
        {
            await _loggerService.LogErrorRegisterAsync(new LogErrorModel
            {
                Exception = new Exception(),
                Message = $"RabbitMQ; PublishCommandAsync: PublisherSetup não encontrado para {commandName_FullName}",
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
                { "CommandName_FullName", commandName_FullName },
                { "CommandName", commandName },
                { "CurrentContext", _environmentSettings.CurrentContext }
            };
        basicProperties.DeliveryMode = 2;

        if (publishSetup.Priority.HasValue)
            basicProperties.Priority = publishSetup.Priority.Value;

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
                Message = "RabbitMQ; PublishCommandAsync: Erro ao publicar mensagem",
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
