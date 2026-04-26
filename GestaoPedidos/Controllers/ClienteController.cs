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
        

        [HttpGet ("GetCliente")]
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



        //[HttpGet (Name = "GetByID")]
        //public async Task<IActionResult> GetClientePorId()
        //{
        //    var conn = _configuration.GetConnectionString("DefaultConnection");

        //    await using (var connection = new SqlConnection(conn))
        //    {
        //        await connection.ExecuteAsync("SELECT * FROM TB_CLIENTE WHERE ID = @ID", new SqlParameter() { Value = 1, ParameterName = "ID" });
        //    }
        //    return Ok();

        //}
    }
}
