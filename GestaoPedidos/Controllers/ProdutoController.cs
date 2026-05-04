using Dapper;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using System.Data;

namespace GestaoPedidos.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProdutoController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public ProdutoController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        [HttpGet("GetProduto")]
        public async Task<IActionResult> GetProduto()
        {
            var conn = _configuration.GetConnectionString("DefaultConnection");
            IDbConnection connection = new MySqlConnection(conn);

            var sql = "SELECT * FROM TB_PRODUTO";

            var produtos = await connection.QueryAsync<Produto>(sql);

            return Ok(produtos);
        }


        [HttpPost]
        public async Task<IActionResult> CriarProduto([FromBody] Produto produto)
        {
            var conn = _configuration.GetConnectionString("DefaultConnection");
            IDbConnection connection = new MySqlConnection(conn);

            var sql = "INSERT INTO TB_PRODUTO (NOME, VALOR, ESTOQUE) VALUES (@Nome, @Valor, @Estoque)";

            await connection.ExecuteAsync(sql, produto);

            return Ok();
        }



        [HttpGet ("{id:int}")]
        public async Task<ActionResult<Produto>> GetProdutoById(int id)
        {
            var conn = _configuration.GetConnectionString("DefaultConnection");
            using var connection = new MySqlConnection(conn);
            const string sql = "SELECT * FROM TB_PRODUTO WHERE Id = @id";
            
            var produto = await connection.QueryFirstOrDefaultAsync<Produto>(sql, new { Id = id });

            if (produto == null)
            {
                return NotFound();
            }
            return Ok(produto);
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteProduto(int id)
        {
            // Define the MySQL connection string
            var conn = _configuration.GetConnectionString("DefaultConnection");

            using (var connection = new MySqlConnection(conn))
            {
                string sql = "DELETE FROM TB_PRODUTO WHERE id = @Id";

                // Execute the delete operation
                int rowsAffected = connection.Execute(sql, new { Id = id });

                if (rowsAffected > 0)
                {
                    return NoContent(); // 204
                }
                return NotFound(); // 404
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateProduto(int id, [FromBody] Produto produto)
        {
            string sql = "UPDATE TB_PRODUTO SET nome = @Nome, valor = @Valor, estoque = @Estoque WHERE id = @Id";

            var conn = _configuration.GetConnectionString("DefaultConnection");

            using (var connection = new MySqlConnection(conn))
            {
                // ExecuteAsync updates the record efficiently [7]
                var affectedRows = await connection.ExecuteAsync(sql, new
                {
                    produto.Nome,
                    produto.Valor,
                    produto.Estoque,
                    Id = id
                });

                if (affectedRows == 0) return NotFound();
                return NoContent();
            }
        }

    }
}