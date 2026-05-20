using Dapper;
using Domain.Entities;
using Domain.Interfaces;
using GestaoPedidos.Infrastructure.Data;
using Mysqlx.Crud;
using Org.BouncyCastle.Utilities.Zlib;
using System.Data;

namespace Infrastructure.Repositories;

public class ProdutoRepository : IProdutoRepository
{
    private readonly IDbConnection _connection;

    public ProdutoRepository(DbConnectionFactory factory)
    {
        _connection = factory.CreateConnection();
    }

    public async Task<List<Produto>> Listar()
    {
        var sql = "SELECT * FROM TB_PRODUTO";
        var result = await _connection.QueryAsync<Produto>(sql);
        return result.ToList();
    }

    public async Task<Produto?> BuscarPorId(int id)
    {
        var sql = "SELECT * FROM TB_PRODUTO WHERE ID = @id";
        return await _connection.QueryFirstOrDefaultAsync<Produto>(sql, new { Id = id });
    }

    public async Task Criar(Produto produto)
    {
        var sql = "INSERT INTO TB_PRODUTO (NOME, VALOR, ESTOQUE) VALUES (@Nome, @Valor, @Estoque)";

        await _connection.ExecuteAsync(sql, produto);
    }

    public async Task Atualizar(Produto produto)
    {
        var sql = "UPDATE TB_PRODUTO SET nome = @Nome, valor = @Valor, estoque = @Estoque WHERE id = @Id";

        await _connection.ExecuteAsync(sql, produto);
    }

    public async Task Deletar(int id)
    {
        var sql = "DELETE FROM TB_PRODUTO WHERE id = @Id";

        await _connection.QueryFirstOrDefaultAsync<Pedido>(sql, new { Id = id });
    }
}