using MediatR;
using Poc.Microsservicosv2.Base.ApplicationLayer.ApplicationsServices;
using Poc.Microsservicosv2.Catalogo.Domain.Events;

namespace Poc.Microsservicosv2.Catalogo.Application.Services.ValidarPedidoCriado;

public sealed class ValidarPedidoCriadoAppService : AppServiceBase, IRequestHandler<ValidarPedidoCriadoRequest, bool>
{
    public ValidarPedidoCriadoAppService(DependenciesAppServiceBase dependencies) : base(dependencies)
    {
    }

    public async Task<bool> Handle(ValidarPedidoCriadoRequest request, CancellationToken cancellationToken)
    {
        //recuperar produto repositório

        //validação

        //EstoqueRejeitadoParaPedidoEvent

        var estoqueConfirmadoParaPedidoEvent = new EstoqueConfirmadoParaPedidoEvent
        {
            IdPedido = request.IdPedido
        };

        await SendEventToQueueAsync(estoqueConfirmadoParaPedidoEvent);

        return true;
    }
}
