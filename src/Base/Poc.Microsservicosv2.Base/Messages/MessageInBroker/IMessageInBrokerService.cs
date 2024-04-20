using Poc.Microsservicosv2.Base.Messages.MessageInBroker.Models;
using System.Data.SqlClient;

namespace Poc.Microsservicosv2.Base.Messages.MessageInBroker;

public interface IMessageInBrokerService
{
    MessageInBrokerModel CreateMessage(string fullName, string name, string currentContext, string body, bool isEvent, string originalContext, int? messageIdReference, SqlConnection sqlConnection, SqlTransaction sqlTransaction, string queue = null);

    MessageInBrokerModel GetMessageByMessageId(int messageId, SqlConnection sqlConnection, SqlTransaction sqlTransaction);

    MessageInBrokerModel GetMessageByMessageIdReference(int messageIdReference, SqlConnection sqlConnection, SqlTransaction sqlTransaction, string queue = null);

    IEnumerable<MessageInBrokerModel> GetMessagesToPublish(bool isEvent, int secondsDelay, SqlConnection sqlConnection, SqlTransaction sqlTransaction);

    void IncrementNum(MessageInBrokerModel message, SqlConnection sqlConnection, SqlTransaction sqlTransaction);

    void MarkAsProcessed(MessageInBrokerModel message, SqlConnection sqlConnection, SqlTransaction sqlTransaction);

    void MarkAsMessageInBroker(MessageInBrokerModel message, SqlConnection sqlConnection, SqlTransaction sqlTransaction);
}
