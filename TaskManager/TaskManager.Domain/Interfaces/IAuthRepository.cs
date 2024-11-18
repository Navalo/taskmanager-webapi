using TaskManager.Domain.DTOs;

namespace TaskManager.Domain.Interfaces
{
    public interface IAuthRepository
    {
        Task<bool> SignInAsync(LoginRequestDto user);
        Task<bool> RegisterAsync(RegisterRequestDto user);
    }
}
