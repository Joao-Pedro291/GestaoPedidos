using System.Data;
using MySql.Data.MySqlClient;

namespace GestaoPedidos.Infrastructure.Data;

public class DbConnectionFactory
{
    private readonly string _connectionString;

    public DbConnectionFactory(string connectionString)
    {
        _connectionString = connectionString;
    }

    public IDbConnection CreateConnection()
        => new MySqlConnection(_connectionString);
}