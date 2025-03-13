using APIRestaurante.Domain.Entities;
using APIRestaurante.Application.DTOs;

namespace APIRestaurante.Application.Interfaces
{
    public interface IAuthService
    {
        Task<User> RegisterAsync(RegisterDTO registerDto);
        Task<string> LoginAsync(LoginDTO loginDto);
    }
}
