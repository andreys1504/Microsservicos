using Microsoft.AspNetCore.Mvc;
using Poc.Microsservicosv2.Base.PresentationLayer.AspNet.ControllerBase;
using Poc.Microsservicosv2.Pedidos.Application.Services.NovoPedido;

namespace Poc.Microsservicosv2.Pedidos.Api.Controllers;

[ApiController]
[Route("pedidos")]
public class PedidoController : ApiControllerBase
{
    public PedidoController(DependenciesApiControllerBase dependencies) : base(dependencies)
    {
    }


    [HttpPost]
    public async Task<IActionResult> NovoPedido()
    {
        var request = new NovoPedidoRequest();

        await base.SendCommandToQueueAsync(request);

        return Ok();
    }

    [HttpGet("{idPedido}")]
    public IActionResult Pedido(Guid idPedido)
    {
        return Ok(new
        {
            IdPedido = idPedido,
        });
    }
}
