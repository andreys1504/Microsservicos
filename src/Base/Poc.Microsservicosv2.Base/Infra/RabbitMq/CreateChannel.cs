using RabbitMQ.Client;

namespace Poc.Microsservicosv2.Base.Infra.RabbitMq;

public static class CreateChannel
{
    public static IModel Create(IConnection connectionRabbitMq)
    {
        //TODO: Pendência Close/Dispose Canais

        return connectionRabbitMq.CreateModel();
    }
}
