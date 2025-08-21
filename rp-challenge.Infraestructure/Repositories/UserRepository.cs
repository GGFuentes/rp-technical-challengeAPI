using Dapper;
using rp_challenge.Domain.Entities;
using rp_challenge.Domain.Repositories;
using rp_challenge.Infraestructure.Data;

namespace rp_challenge.Infraestructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public UserRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();

            const string sql = @"
            SELECT id, email, name, password, created, updated
            FROM users 
            WHERE id = @Id";

            var result = await connection.QuerySingleOrDefaultAsync<dynamic>(sql, new { Id = id });

            return result == null ? null : MapFromDatabase(result);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();

            const string sql = @"
            SELECT id, email, username, password, created_at, updated_at
            FROM users 
            WHERE email = @Email";

            var result = await connection.QuerySingleOrDefaultAsync<dynamic>(sql, new { Email = email });

            return result == null ? null : MapFromDatabase(result);
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();

            const string sql = @"
            SELECT id, email, username, password, created_at, updated_at
            FROM users 
            WHERE username = @Username";

            var result = await connection.QuerySingleOrDefaultAsync<dynamic>(sql, new { Username = username });

            return result == null ? null : MapFromDatabase(result);
        }

        public async Task<int> CreateAsync(User user)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();

            const string sql = @"
            INSERT INTO users (email, username, password, created_at, updated_at)
            VALUES (@Email, @Username, @Password, @Created, @Updated)
            RETURNING id";

            var id = await connection.QuerySingleAsync<int>(sql, new
            {
                Email = user.Email,
                Name = user.Name,
                Password = user.Password,
                Create = user.Created,
                Updated = user.Updated
            });

            return id;
        }

        public async Task<bool> UpdateAsync(User user)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();

            const string sql = @"
            UPDATE users 
            SET email = @Email, username = @Username, password = @Password, 
                updated_at = @Updated
            WHERE id = @Id";

            var affectedRows = await connection.ExecuteAsync(sql, new
            {
                Id = user.Id,
                Email = user.Email,
                Username = user.Name,
                Password = user.Password,
                Updated = user.Updated
            });

            return affectedRows > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();

            const string sql = "DELETE FROM users WHERE id = @Id";

            var affectedRows = await connection.ExecuteAsync(sql, new { Id = id });

            return affectedRows > 0;
        }

        public async Task<bool> ExistsByEmailAsync(string email)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();

            const string sql = "SELECT COUNT(1) FROM users WHERE email = @Email";

            var count = await connection.QuerySingleAsync<int>(sql, new { Email = email });

            return count > 0;
        }

        public async Task<bool> ExistsByUsernameAsync(string username)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();

            const string sql = "SELECT COUNT(1) FROM users WHERE username = @Username";

            var count = await connection.QuerySingleAsync<int>(sql, new { Username = username });

            return count > 0;
        }

        private static User MapFromDatabase(dynamic row)
        {
            return User.FromDatabase(
                (int)row.id,
                (string)row.email,
                (string)row.username,
                (string)row.password,
                (DateTime)row.created,
                (DateTime)row.updated
            );
        }
    }
}
