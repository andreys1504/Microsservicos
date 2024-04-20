using Microsoft.AspNetCore.Mvc;

namespace Poc.Microsservicosv2.Catalogo.Api.Controllers
{
    [ApiController]
    [Route("produto")]
    public class ProdutoController : ControllerBase
    {
        [HttpGet("preco-produto/{idProduto}")]
        public IActionResult PrecoProduto(Guid idProduto)
        {
            return Ok((decimal)153.95);
        }
    }
}
