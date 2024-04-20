using RabbitMQ.Client;

namespace Poc.Microsservicosv2.Base.Infra.RabbitMq;

public class CreateConnection
{
    public static IConnection Create(ConnectionFactory connectionFactory)
    {
        //TODO: Pendência Close/Dispose Canal

        return connectionFactory.CreateConnection();
    }
}
