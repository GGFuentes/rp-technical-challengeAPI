using Npgsql;
using System.Data;

namespace rp_challenge.Infraestructure.Data
{
    public class PostgreSqlConnectionFactory : IDbConnectionFactory
    {
        private readonly string _connectionString;

        public PostgreSqlConnectionFactory(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public async Task<IDbConnection> CreateConnectionAsync()
        {
            var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();
            return connection;
        }
    }
}
