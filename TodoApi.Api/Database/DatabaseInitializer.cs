using Npgsql;
using System.Data;
using Dapper;

namespace TodoApi.Api.Database
{
    public class DatabaseInitializer
    {
        private readonly string _connectionString;

        public DatabaseInitializer(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task InitializeAsync()
        {
            using var connection = await CreateConnectionAsync();
            await connection.ExecuteAsync(@"CREATE TABLE IF NOT EXISTS Todo (
                Id UUID PRIMARY KEY, 
                Task TEXT NOT NULL,
                Description TEXT NOT NULL,
                Is_Complete BOOLEAN NOT NULL
                )");
        }

        public async Task<IDbConnection> CreateConnectionAsync()
        {
            var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();
            return connection;
        }
    }
}
