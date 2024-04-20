using MediatR;
using Poc.Microsservicosv2.Base.ApplicationLayer.ApplicationsServices;
using Poc.Microsservicosv2.Base.Infra.Services.ServicesInMicrosservicosv2.Catalogo;
using Poc.Microsservicosv2.Pedidos.Domain.Events;

namespace Poc.Microsservicosv2.Pedidos.Application.Services.NovoPedido;

public sealed class NovoPedidoAppService : AppServiceBase, IRequestHandler<NovoPedidoRequest, bool>
{
    private readonly ICatalogoService _catalogoService;

    public NovoPedidoAppService(
        DependenciesAppServiceBase dependencies,
        ICatalogoService catalogoService) : base(dependencies)
    {
        _catalogoService = catalogoService;
    }

    public async Task<bool> Handle(NovoPedidoRequest request, CancellationToken cancellationToken)
    {
        //cria novo pedido
        var pedido = new
        {
            Id = Guid.NewGuid(),
            IdProduto = request.IdProduto,
            DataHoraRealizacaoPedido = request.Timestamp
        };


        //Salva pedido repositorio
        decimal preco = await _catalogoService.PrecoProduto(request.IdProduto);


        //Notifica todo o sistema que um pedido foi realizado com sucesso
        var statusPedidoAlteradoParaRealizadoEvent = new PedidoCriadoEvent
        {
            IdPedido = pedido.Id,
            IdProduto = pedido.IdProduto,
            DataHoraRealizacaoPedido = pedido.DataHoraRealizacaoPedido,
            Quantidade = 1
        };
        await base.SendEventToQueueAsync(statusPedidoAlteradoParaRealizadoEvent);

        return true;
    }
}
