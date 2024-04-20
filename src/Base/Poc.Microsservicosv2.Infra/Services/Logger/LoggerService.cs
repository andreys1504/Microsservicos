using Poc.Microsservicosv2.Base.Infra.Services.Logger;
using Poc.Microsservicosv2.Base.Infra.Services.Logger.Models;
using Poc.Microsservicosv2.Base.Settings;
using System.Text;

namespace Poc.Microsservicosv2.Infra.Services.Logger;

public sealed class LoggerService<TSource> : ILoggerService<TSource> where TSource : class
{
    private readonly EnvironmentSettings _environmentSettings;

    public LoggerService(EnvironmentSettings environmentSettings)
    {
        _environmentSettings = environmentSettings;
    }

    public Task LogErrorRegisterAsync(LogErrorModel log)
    {
        //TODO: configurar banco de dados
        string local = typeof(TSource).FullName;

        string context = _environmentSettings.CurrentContext;
        string project = _environmentSettings.CurrentProjectShortName;
        var message1 = log.Message;
        var messagesExceptions = new StringBuilder();
        var stackTrace = new StringBuilder();
        
        IEnumerable<Exception> exceptions = GetAllExceptions(log.Exception);
        var divider = "\n\n\n\n==============||==============\n\n\n\n";
        foreach (Exception exception in exceptions)
        {
            messagesExceptions.Append($"{exception.Message}; ");

            stackTrace.AppendLine(divider);
            stackTrace.AppendLine(exception.StackTrace);
            stackTrace.AppendLine(divider);
        }


        /*
            CurrentContext: context, //Pedidos
            CurrentProject: project, //Pedidos.Api
            Level: nameof(log.Level)
            Message1: message1,
            Message2: log.Exception.GetType().Name,
            Message3: exceptionsMessages,
            Message4: local
            Data: stackTrace,
            Timestamp: DateTime.UtcNow,
            Keyword: log.Keyword
         */

        return Task.CompletedTask;
    }

    //
    private IEnumerable<Exception> GetAllExceptions(Exception exception)
    {
        do
        {
            yield return exception;
            exception = exception.InnerException;
        } while (exception is not null);
    }
}
