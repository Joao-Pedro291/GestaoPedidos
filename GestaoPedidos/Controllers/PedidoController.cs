using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using MySqlConnector;
using System.Data;

namespace GestaoPedidos.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PedidoController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public PedidoController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        [HttpGet("GetPedido")]
        public async Task<IActionResult> GetPedido()
        {
            var conn = _configuration.GetConnectionString("DefaultConnection");
            IDbConnection connection = new MySqlConnection(conn);

            var sql = "SELECT * FROM TB_PEDIDO";

            var pedidos = await connection.QueryAsync<Pedido>(sql);

            return Ok(pedidos);
        }


        [HttpPost]
        public async Task<IActionResult> CriarPedido([FromBody] Pedido pedido)
        {
            var conn = _configuration.GetConnectionString("DefaultConnection");
            IDbConnection connection = new MySqlConnection(conn);

            var sql = "INSERT INTO TB_PEDIDO (ID_CLIENTE, ID_PRODUTO, QUANTIDADE, VALOR_TOTAL, DATA_PEDIDO) VALUES (@ClienteId, @ProdutoId, @Quantidade, @ValorTotal, @DataPedido)";

            await connection.ExecuteAsync(sql, pedido);

            return Ok();
        }



        [HttpGet("{id:int}")]
        public async Task<ActionResult<Cliente>> GetClientById(int id)
        {
            // Replace with your actual connection string
            var conn = _configuration.GetConnectionString("DefaultConnection");
            using var connection = new MySqlConnection(conn);
            const string sql = "SELECT * FROM TB_CLIENTE WHERE Id = @id";
            // QueryFirstOrDefaultAsync handles opening/closing if needed, 
            // but wrapped in 'using' is best practice for connection management [2]
            var client = await connection.QueryFirstOrDefaultAsync<Cliente>(sql, new { Id = id });

            if (client == null)
            {
                return NotFound();
            }
            return Ok(client);
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteCliente(int id)
        {
            // Define the MySQL connection string
            var conn = _configuration.GetConnectionString("DefaultConnection");

            using (var connection = new MySqlConnection(conn))
            {
                string sql = "DELETE FROM TB_CLIENTE WHERE id = @Id";

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
        public async Task<IActionResult> UpdateCliente(int id, [FromBody] Cliente cliente)
        {
            string sql = "UPDATE TB_CLIENTE SET nome = @Nome, email = @Email WHERE id = @Id";

            var conn = _configuration.GetConnectionString("DefaultConnection");

            using (var connection = new MySqlConnection(conn))
            {
                // ExecuteAsync updates the record efficiently [7]
                var affectedRows = await connection.ExecuteAsync(sql, new
                {
                    cliente.Nome,
                    cliente.Email,
                    Id = id
                });

                if (affectedRows == 0) return NotFound();
                return NoContent();
            }
        }
        //        using (var command = new MySqlCommand(sql))
        //        {
        //            command.Parameters.AddWithValue("@id", id);

        //            }

        //return NotFound();

        //await using (var conn = new SqlConnection(conn))
        //{
        //    await connection.ExecuteAsync("SELECT * FROM TB_CLIENTE WHERE ID = @ID", new SqlParameter() { Value = 1, ParameterName = "ID" });
        //}
        //return Ok();

    }
}

