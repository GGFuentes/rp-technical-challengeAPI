using rp_challenge.Domain.Entities;

namespace rp_challenge.Domain.Repositories
{
    public interface ICarRepository
    {
        Task<Car?> GetByIdAsync(int id);
        Task<IEnumerable<Car>> GetAllAsync();
        Task<Car?> GetByModelAsync(string model);
        Task<int> CreateAsync(Car car);
        Task<bool> UpdateAsync(Car car);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
