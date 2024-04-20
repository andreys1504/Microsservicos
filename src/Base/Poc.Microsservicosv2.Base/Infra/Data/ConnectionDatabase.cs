using Poc.Microsservicosv2.Base.Settings;
using System.Data.SqlClient;

namespace Poc.Microsservicosv2.Base.Infra.Data;

public sealed class ConnectionDatabase
{
    public static SqlConnection NewConnection(EnvironmentSettings environmentSettings)
    {
        var sqlConnection = new SqlConnection(connectionString: environmentSettings.DatabaseConnectionString);
        sqlConnection.Open();
        return sqlConnection;
    }
}
