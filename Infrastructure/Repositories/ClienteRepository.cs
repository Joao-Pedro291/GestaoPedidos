using Dapper;
using Domain.Entities;
using Domain.Interfaces;
using GestaoPedidos.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Infrastructure.Repositories;

public class ClienteRepository : IClienteRepository
{
    private readonly IDbConnection _connection;

    public ClienteRepository(DbConnectionFactory factory)
    {
        _connection = factory.CreateConnection();
    }

    public async Task<List<Cliente>> Listar()
    {
        var sql = "SELECT * FROM tb_cliente";
        var result = await _connection.QueryAsync<Cliente>(sql);
        return result.ToList();
    }
    public async Task<Cliente?> BuscarPorId(int id)
    {
        var sql = "SELECT * FROM TB_CLIENTE WHERE Id = @id";
        return await _connection.QueryFirstOrDefaultAsync<Cliente>(sql, new { Id = id });
    }

    public async Task Criar(Cliente cliente)
    {
        var sql = "INSERT INTO TB_CLIENTE (NOME, EMAIL) VALUES (@Nome, @Email)";

        await _connection.ExecuteAsync(sql, cliente);
    }

    public async Task Atualizar(Cliente cliente)
    {
        var sql = "UPDATE TB_CLIENTE SET nome = @Nome, email = @Email WHERE id = @Id";

        await _connection.ExecuteAsync(sql, cliente);
    }

    public async Task Deletar(int id)
    {
        var sql = "DELETE FROM TB_CLIENTE WHERE id = @Id";

        await _connection.QueryFirstOrDefaultAsync<Cliente>(sql, new { Id = id });
    }


}
