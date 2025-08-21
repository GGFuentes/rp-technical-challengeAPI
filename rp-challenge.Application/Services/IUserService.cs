using rp_challenge.Application.DTOs;

namespace rp_challenge.Application.Services
{
    public interface IUserService
    {
        Task<UserDTO> CreateAsync(CreateUserDTO createUserDto);
        Task<LoginResponseDTO> LoginAsync(LoginDTO loginDto);
        Task<UserDTO?> GetByIdAsync(int id);
    }
}
