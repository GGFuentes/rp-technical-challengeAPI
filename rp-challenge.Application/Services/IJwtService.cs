using rp_challenge.Application.DTOs;

namespace rp_challenge.Application.Services
{
    public interface IJwtService
    {
        string GenerateToken(UserDTO user);
        int? ValidateToken(string token);
    }
}
