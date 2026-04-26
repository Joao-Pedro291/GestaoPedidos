using Microsoft.AspNetCore.Mvc;

namespace GestaoPedidos.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        [HttpGet ]
        public IActionResult GetProduto()
        {
            // Simulando busca no banco
            var cliente = new Cliente { Nome = "João" };

            if (cliente == null)
            {
                return NotFound(); // Retorna 404 se não encontrar
            }

            return Ok(cliente); // Retorna 200 e o objeto em JSON
        }
    }
}
