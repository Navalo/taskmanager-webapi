using TaskManager.Domain.DTOs;
using TaskManager.Domain.Entities;

namespace TaskManager.Domain.Interfaces
{
    public interface IAuthRepository
    {
        Task<User?> SignInAsync(LoginRequestDto user);
        Task<bool> RegisterAsync(RegisterRequestDto user);
    }
}
