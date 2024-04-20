using Poc.Microsservicosv2.Pedidos.Application.Services.NovoPedido;

namespace Poc.Microsservicosv2.Pedidos.Application.MessageBroker.AsyncOperationsOnPedidos;

public static class PublishersCommandsConfig
{
    public static List<Type> Register()
    {
        return
            [
                typeof(NovoPedidoRequest)
            ];
    }
}
