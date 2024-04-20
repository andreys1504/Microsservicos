using Microsoft.AspNetCore.Mvc;

namespace Poc.Microsservicosv2.Pagamento.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {

        [HttpGet]
        public IActionResult Get()
        {
            return Ok();
        }
    }
}
