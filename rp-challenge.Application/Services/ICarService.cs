using rp_challenge.Application.DTOs;

namespace rp_challenge.Application.Services
{
    public interface ICarService
    {
        Task<CarDTO?> GetByIdAsync(int id);
        Task<IEnumerable<CarDTO>> GetAllAsync();
        Task<CarDTO> CreateAsync(CreateCarDTO createCarDto);
        Task<CarDTO> UpdateAsync(int id, UpdateCarDTO updateCarDto);
        Task<bool> DeleteAsync(int id);
    }
}
