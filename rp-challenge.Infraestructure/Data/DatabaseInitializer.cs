using Npgsql;
using System.Data;

namespace rp_challenge.Infraestructure.Data
{
    public class DatabaseInitializer : IDatabaseInitializer
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public DatabaseInitializer(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task InitializeAsync()
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();

            await CreateTablesAsync(connection);
        }

        private async Task CreateTablesAsync(IDbConnection connection)
        {
            var createUsersTable = @"
            CREATE TABLE IF NOT EXISTS users (
                id SERIAL PRIMARY KEY,
                email VARCHAR(100) UNIQUE NOT NULL,
                username VARCHAR(100) UNIQUE NOT NULL,
                password_hash VARCHAR(255) NOT NULL,
                created TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
                updated TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW()
            );

            CREATE INDEX IF NOT EXISTS idx_users_email ON users(email);
            CREATE INDEX IF NOT EXISTS idx_users_username ON users(username);
        ";

            var createCarsTable = @"
            CREATE TABLE IF NOT EXISTS cars (
                id SERIAL PRIMARY KEY,
                brand VARCHAR(60) NOT NULL,
                model VARCHAR(50) NOT NULL,
                price DECIMAL UNIQUE NOT NULL,
                created TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
                updated TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW()
            );

            CREATE INDEX IF NOT EXISTS idx_cars_model ON cars(model);
        ";

            if (connection is NpgsqlConnection npgsqlConnection)
            {
                using var command = new NpgsqlCommand(createUsersTable + createCarsTable, npgsqlConnection);
                await command.ExecuteNonQueryAsync();
            }
            else
            {
                
                using var command = connection.CreateCommand();
                command.CommandText = createUsersTable + createCarsTable;
                command.ExecuteNonQuery();
            }
        }
    }
}
