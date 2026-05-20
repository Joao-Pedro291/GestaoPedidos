using Dapper;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using System.Data;

namespace GestaoPedidos.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PedidoController : ControllerBase
    {
        private readonly PedidoService _service;
        public PedidoController(PedidoService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Criar(CriarPedidoDTO dto)
        {
            await _service.Criar(dto);
            return Ok();
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Atualizar(int id, Pedido pedido)
        {
            if (id != pedido.PedidoId)
            {
                return BadRequest("O id da rota é diferente do id do pedido");
            }

            var pedidoAtualizado = await _service.Atualizar(pedido);

            if (pedidoAtualizado != null)
            {
                return NotFound("O pedido não foi encontrado");
            }

            return Ok(pedidoAtualizado);
        }

        [HttpGet] 
        public async Task<IActionResult> Listar()
        {
            var pedidos = await _service.Listar();

            return Ok(pedidos);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult> GetById(int id)
        {
            var pedido = await _service.BuscarPorId(id);

            if (pedido == null)
            {
                return NotFound("Cliente não encontrado");
            }

            return Ok(pedido);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var resultado = await _service.Deletar(id);

            if (!resultado.Sucesso)
            {
                return BadRequest(resultado.Erro);
            }

            return NoContent();
        }

    }
}
