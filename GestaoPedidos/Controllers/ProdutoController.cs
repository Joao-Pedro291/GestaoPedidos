using Dapper;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using System.Data;

namespace GestaoPedidos.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProdutoController : ControllerBase
    {
        private readonly ProdutoService _service;
        public ProdutoController(ProdutoService service)
        {
            _service = service;
        }


        [HttpPost("PostProduto")]
        public async Task<IActionResult> Criar(CriarProdutoDTO dto)
        {
            await _service.Criar(dto);
            return Ok();
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Atualizar(int id, Produto produto)
        {
            if (id != produto.Id)
            {
                return BadRequest("O id da rota é diferente do id do produto");
            }

            var produtoAtualizado = await _service.Atualizar(produto);

            if (produtoAtualizado == null)
            {
                return NotFound("Produto não encontrado");
            }

            return Ok(produtoAtualizado);
        }

        [HttpGet("GetProduto")]
        public async Task<IActionResult> Listar()
        {
            var produtos = await _service.Listar();

            return Ok(produtos);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult> GetById(int id) 
        { 
            var produto = await _service.BuscarPorId(id);

            if (produto == null) 
            {
                return NotFound("Produto não encontrado");
            }

            return Ok(produto);
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
