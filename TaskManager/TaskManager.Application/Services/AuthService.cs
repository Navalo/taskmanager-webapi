using TaskManager.Domain.DTOs;
using TaskManager.Domain.Interfaces;

namespace TaskManager.Application.Services
{
    public class AuthService(IAuthRepository authRepository) : IAuthService
    {
        private readonly IAuthRepository _authRepository = authRepository;

        public async Task<bool> RegisterAsync(RegisterRequestDto user)
        {
            return await _authRepository.RegisterAsync(user);
        }

        public async Task<bool> SignInAsync(LoginRequestDto user)
        {
            return await _authRepository.SignInAsync(user);
        }

    }
}
