using Dapper;
using rp_challenge.Domain.Entities;
using rp_challenge.Domain.Repositories;
using rp_challenge.Infraestructure.Data;

namespace rp_challenge.Infraestructure.Repositories
{
    public class CarRepository : ICarRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public CarRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<Car?> GetByIdAsync(int id)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();

            const string sql = @"
            SELECT id, brand, model, created, updated
            FROM cars 
            WHERE id = @Id";

            var result = await connection.QuerySingleOrDefaultAsync<dynamic>(sql, new { Id = id });

            return result == null ? null : MapFromDatabase(result);
        }

        public async Task<IEnumerable<Car>> GetAllAsync()
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();

            const string sql = @"
            SELECT id, brand, model, price, created, updated
            FROM cars 
            ORDER BY created_at DESC";

            var results = await connection.QueryAsync<dynamic>(sql);

            return results.Select(MapFromDatabase);
        }

        public async Task<Car?> GetByModelAsync(string model)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();

            const string sql = @"
            SELECT id, brand, model, price, created, updated
            FROM cars 
            WHERE model = @Model";

            var result = await connection.QuerySingleOrDefaultAsync<dynamic>(sql, new { Model = model });

            return result == null ? null : MapFromDatabase(result);
        }

        public async Task<int> CreateAsync(Car car)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();

            const string sql = @"
            INSERT INTO cars (brand, model, created, updated)
            VALUES (@Brand, @Model, @Created, @CreatedAt, @Updated)
            RETURNING id";

            var id = await connection.QuerySingleAsync<int>(sql, new
            {
                Brand = car.Brand,
                Model = car.Model,
                Price = car.Price,
                Created = car.Created,
                Updated = car.Updated
            });

            return id;
        }

        public async Task<bool> UpdateAsync(Car car)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();

            const string sql = @"
            UPDATE cars 
            SET model = @Model, brand = @Brand, price = @Price, 
                publication_date = @PublicationDate, updated_at = @UpdatedAt
            WHERE id = @Id";

            var affectedRows = await connection.ExecuteAsync(sql, new
            {
                Id = car.Id,
                Brand = car.Brand,
                Model = car.Model,
                Price = car.Price,
                Updated = car.Updated
            });

            return affectedRows > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();

            const string sql = "DELETE FROM cars WHERE id = @Id";

            var affectedRows = await connection.ExecuteAsync(sql, new { Id = id });

            return affectedRows > 0;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();

            const string sql = "SELECT COUNT(1) FROM cars WHERE id = @Id";

            var count = await connection.QuerySingleAsync<int>(sql, new { Id = id });

            return count > 0;
        }

        private static Car MapFromDatabase(dynamic row)
        {
            return Car.FromDatabase(
                (int)row.id,
                (string)row.brand,
                (string)row.model,
                (decimal)row.price,
                (DateTime)row.created,
                (DateTime)row.updated
            );
        }
    }
}
