using Poc.Microsservicosv2.Base.Infra.Services.Logger.Models;

namespace Poc.Microsservicosv2.Base.Infra.Services.Logger;

public interface ILoggerService<TSource> where TSource : class
{
    Task LogErrorRegisterAsync(LogErrorModel log);
}
