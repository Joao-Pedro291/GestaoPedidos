using Dapper;
using Domain.Entities;
using Domain.Interfaces;
using GestaoPedidos.Infrastructure.Data;
using Mysqlx.Crud;
using Org.BouncyCastle.Utilities.Zlib;
using System.Data;

namespace Infrastructure.Repositories;

public class PedidoRepository : IPedidoRepository
{
    private readonly IDbConnection _connection;

    public PedidoRepository(DbConnectionFactory factory)
    {
        _connection = factory.CreateConnection();
    }

    public async Task<List<Pedido>> Listar()
    {
        var sql = "SELECT * FROM TB_PEDIDO";
        var result = await _connection.QueryAsync<Pedido>(sql);
        return result.ToList();
    }

    public async Task<Pedido?> BuscarPorId(int id)
    {
        var sql = "SELECT * FROM TB_PEDIDO WHERE PEDIDOID = @PedidoId";
        return await _connection.QueryFirstOrDefaultAsync<Pedido>(sql, new { PedidoId = id });
    }

    public async Task Criar(Pedido pedido)
    {
        var sql = "INSERT INTO TB_PEDIDO(CLIENTEID, PRODUTOID, QUANTIDADE, VALORTOTAL, DATAPEDIDO) VALUES(@ClienteId, @ProdutoId, @Quantidade, @ValorTotal, @DataPedido)";

        await _connection.ExecuteAsync(sql, pedido);
    }

    public async Task Atualizar(Pedido pedido)
    {
        var sql = "UPDATE TB_PEDIDO SET ClienteId = @ClienteId, ProdutoId = @ProdutoId, QUANTIDADE = @Quantidade, ValorTotal = @ValorTotal, DataPedido = @DataPedido  WHERE PedidoId = @PedidoId";

        await _connection.ExecuteAsync(sql, pedido);
    }

    public async Task Deletar(int id)
    {
        var sql = "DELETE FROM TB_PEDIDO WHERE PEDIDOID = @Id";

        await _connection.QueryFirstOrDefaultAsync<Pedido>(sql, new { Id = id });
    }

    public async Task<bool> ExistePedidoComProduto(int produtoId)
    {
        var sql = @"
        SELECT COUNT(*)
        FROM TB_PEDIDO
        WHERE ProdutoId = @produtoId";

        var quantidade = await _connection.ExecuteScalarAsync<int>(
             sql,
             new { produtoId }
         );
        return quantidade > 0;
    }

    public async Task<bool> ExistePedidoComCliente(int clienteId)
    {
        var sql = @"
        SELECT COUNT(*)
        FROM TB_PEDIDO
        WHERE ClienteId = @clienteId";

        var quantidade = await _connection.ExecuteScalarAsync<int>(
             sql,
             new { clienteId }
         );
        return quantidade > 0;
    }

}