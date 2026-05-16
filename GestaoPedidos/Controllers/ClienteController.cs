using Dapper;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using MySqlConnector;
using System.Data;

namespace GestaoPedidos.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClienteController : ControllerBase
    {
        private readonly ClienteService _service;

        public ClienteController(ClienteService service)
        {
            _service = service;
        }

        [HttpPost("PostCliente")]
        public async Task<IActionResult> Criar(CriarClienteDTO dto)
        { 
            await _service.Criar(dto);
            return Ok();
        }

        [HttpPut("PutCliente")]
        public async Task<IActionResult> Atualizar(int id, Cliente cliente)
        {
            if (id != cliente.Id)
            {
                return BadRequest("O id da rota é diferente do id do cliente");
            }

            var clienteAtualizado = await _service.Atualizar(cliente);

            if (clienteAtualizado == null)
            {
                return NotFound("Cliente não encontrado");
            }

            return Ok(clienteAtualizado);

        }

        [HttpGet("GetCliente")]
        public async Task<IActionResult> Listar()
        {
            var clientes = await _service.Listar();

            return Ok(clientes);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult> GetById(int id)
        {
            var cliente = await _service.BuscarPorId(id);

            if (cliente == null)
            {
                return NotFound("Cliente não encontrado");
            }

            return Ok(cliente);
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