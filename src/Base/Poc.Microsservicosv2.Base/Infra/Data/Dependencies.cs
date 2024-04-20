using Microsoft.Extensions.DependencyInjection;
using Poc.Microsservicosv2.Base.ResiliencePolicies;
using Poc.Microsservicosv2.Base.Settings;
using System.Data.SqlClient;

namespace Poc.Microsservicosv2.Base.Infra.Data;

public static class Dependencies
{
    public static void Register(IServiceCollection services, EnvironmentSettings environmentSettings)
    {
        services.AddTransientWithRetry<SqlConnection, SqlException>(
            serviceProvider => ConnectionDatabase.NewConnection(environmentSettings), 
            retryCount: 5);
    }
}
