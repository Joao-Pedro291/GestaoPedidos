using Dapper;
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
        private readonly IConfiguration _configuration;
        public ClienteController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        [HttpGet("GetCliente")]
        public async Task<IActionResult> GetCliente()
        {
            var conn = _configuration.GetConnectionString("DefaultConnection");
            IDbConnection connection = new MySqlConnection(conn);

            var sql = "SELECT * FROM tb_cliente";

            var clientes = await connection.QueryAsync<Cliente>(sql);

            return Ok(clientes);
        }


        [HttpPost]
        public async Task<IActionResult> CriarCliente([FromBody] Cliente cliente)
        {
            var conn = _configuration.GetConnectionString("DefaultConnection");
            IDbConnection connection = new MySqlConnection(conn);

            var sql = "INSERT INTO TB_CLIENTE (NOME, EMAIL) VALUES (@Nome, @Email)";

            await connection.ExecuteAsync(sql, cliente);

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
    }
}