using TaskManager.Domain.DTOs;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.Services
{
    public interface IAuthService
    {
        Task<bool> SignInAsync(LoginRequestDto user);
        Task<bool> RegisterAsync(RegisterRequestDto user);
    }
}
