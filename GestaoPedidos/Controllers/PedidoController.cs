using Dapper;
using Microsoft.AspNetCore.Mvc;
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
            if (pedido.Quantidade == 0)
            {
                return BadRequest("A quantidade não pode ser 0");
            }

            var conn = _configuration.GetConnectionString("DefaultConnection");
            IDbConnection connection = new MySqlConnection(conn);

            const string sqlCliente = "SELECT * FROM TB_CLIENTE WHERE Id = @id";
            var client = await connection.QueryFirstOrDefaultAsync<Cliente>(sqlCliente, new { Id = pedido.ClienteId });

            const string sqlProduto = "SELECT * FROM TB_PRODUTO WHERE Id = @id";
            var produto = await connection.QueryFirstOrDefaultAsync<Produto>(sqlProduto, new { Id = pedido.ProdutoId });

            if (pedido.Quantidade > produto.Estoque)
            {
                return BadRequest($"A quantidade nao pode ser maior que o estoque do produto que é {produto.Estoque}");
            }

            if (client != null && produto != null )
            {
                var valor = produto.Valor;
                var valorTotal = valor * pedido.Quantidade;
                pedido.ValorTotal = valorTotal;
                var sql = "INSERT INTO TB_PEDIDO (CLIENTEID, PRODUTOID, QUANTIDADE, VALORTOTAL, DATAPEDIDO) VALUES (@ClienteId, @ProdutoId, @Quantidade, @ValorTotal, @DataPedido)";

                await connection.ExecuteAsync(sql, pedido);

                var sqlUpdateProduto = $"UPDATE TB_PRODUTO SET ESTOQUE = {produto.Estoque - pedido.Quantidade} WHERE ID = {produto.Id}";
                await connection.ExecuteAsync(sqlUpdateProduto);
                return Ok();

            } else { return BadRequest("O cliente ou o produto não existem"); }

        }

        [HttpGet("RelatorioPorCliente/{idUsuario:int}")]
        public async Task<ActionResult<Pedido>> GetPedidoByClienteId(int idUsuario)
        {
            var conn = _configuration.GetConnectionString("DefaultConnection");
            using var connection = new MySqlConnection(conn);
            var sql = $"SELECT tb_pedido.PEDIDOID AS 'IdPedido', " +
                $"tb_cliente.NOME AS 'NomeCliente', " +
                $"tb_produto.NOME AS 'NomeProduto', " +
                $"tb_pedido.ValorTotal AS 'ValorTotal', " +
                $"tb_pedido.DataPedido AS 'DataPedido' " +
                $"FROM tb_pedido " +
                $"INNER JOIN TB_PRODUTO " +
                $"ON TB_PEDIDO.PRODUTOID = tb_produto.ID " +
                $"INNER JOIN TB_CLIENTE " +
                $"ON TB_PEDIDO.CLIENTEID = " +
                $"TB_CLIENTE.ID WHERE TB_PEDIDO.CLIENTEID = {idUsuario}";
            var pedidoLista = await connection.QueryAsync<PedidoResponse>(sql);

            return Ok(pedidoLista);

        }

        [HttpGet("RelatorioPorProduto/{idProduto:int}")]
        public async Task<ActionResult<Pedido>> GetPedidoByProdutoId(int idProduto)
        {
            var conn = _configuration.GetConnectionString("DefaultConnection");
            using var connection = new MySqlConnection(conn);
            var sql = $"SELECT tb_pedido.PEDIDOID AS 'IdPedido', " +
                $"tb_cliente.NOME AS 'NomeCliente', " +
                $"tb_produto.NOME AS 'NomeProduto', " +
                $"tb_pedido.ValorTotal AS 'ValorTotal', " +
                $"tb_pedido.DataPedido AS 'DataPedido' " +
                $"FROM tb_pedido " +
                $"INNER JOIN TB_PRODUTO " +
                $"ON TB_PEDIDO.PRODUTOID = tb_produto.ID " +
                $"INNER JOIN TB_CLIENTE " +
                $"ON TB_PEDIDO.CLIENTEID = " +
                $"TB_CLIENTE.ID WHERE TB_PEDIDO.PRODUTOID = {idProduto}";
            var pedidoLista = await connection.QueryAsync<PedidoResponse>(sql);

            return Ok(pedidoLista);

        }


        [HttpGet("{id:int}")]
        public async Task<ActionResult<Pedido>> GetPedidoById(int id)
        {
            var conn = _configuration.GetConnectionString("DefaultConnection");
            using var connection = new MySqlConnection(conn);
            const string sql = "SELECT * FROM TB_PEDIDO WHERE PEDIDOID = @id";

            var pedido = await connection.QueryFirstOrDefaultAsync<Pedido>(sql, new { Id = id });

            var sqlCliente = $"SELECT * FROM TB_CLIENTE WHERE ID = {pedido.ClienteId}";
            var cliente = await connection.QueryFirstOrDefaultAsync<Cliente>(sqlCliente);

            var sqlProduto = $"SELECT * FROM TB_PRODUTO WHERE ID = {pedido.ProdutoId}";
            var produto = await connection.QueryFirstOrDefaultAsync<Produto>(sqlProduto);

            if (pedido == null)
            {
                return NotFound();
            } else
            {
                var pedidoResponse = new PedidoResponse()
                {
                    IdPedido = pedido.PedidoId,
                    NomeCliente = cliente.Nome,
                    NomeProduto = produto.Nome,
                    ValorTotal = pedido.ValorTotal,
                    DataPedido = pedido.DataPedido
                };
                return Ok(pedidoResponse);
            }
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeletePedido(int id)
        {
            // Define the MySQL connection string
            var conn = _configuration.GetConnectionString("DefaultConnection");

            using (var connection = new MySqlConnection(conn))
            {
                string sql = "DELETE FROM TB_PEDIDO WHERE PEDIDOID = @Id";

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
        public async Task<IActionResult> UpdatePedido(int id, [FromBody] Pedido pedido)
        {
            string sql = "UPDATE TB_PEDIDO SET ClienteId = @ClienteId, ProdutoId = @ProdutoId, QUANTIDADE = @Quantidade, ValorTotal = @ValorTotal, DataPedido = @DataPedido  WHERE PedidoId = @Id";

            var conn = _configuration.GetConnectionString("DefaultConnection");

            using (var connection = new MySqlConnection(conn))
            {
                // ExecuteAsync updates the record efficiently [7]
                var affectedRows = await connection.ExecuteAsync(sql, new
                {
                    pedido.ClienteId,
                    pedido.ProdutoId,
                    pedido.Quantidade,
                    pedido.ValorTotal,
                    pedido.DataPedido,
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

