using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.DTOs;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;
using TaskManager.Infrastructure.Data;

namespace TaskManager.Infrastructure.Repositories
{
    public class AuthRepository(ApplicationDbContext DbContext) : IAuthRepository
    {
        private readonly ApplicationDbContext _DbContext = DbContext;

        public async Task<bool> RegisterAsync(RegisterRequestDto registerRequestDto)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(registerRequestDto);

                var userEntity = new User
                {
                    Password = registerRequestDto.Password,
                    Username = registerRequestDto.Username
                };

                await _DbContext.Users.AddAsync(userEntity);

                var result = await _DbContext.SaveChangesAsync();
                return result > 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<User?> SignInAsync(LoginRequestDto loginDto)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(loginDto);

                var user = await _DbContext.Users.FirstOrDefaultAsync(u => u.Username == loginDto.Username);

                if (user == null)
                {
                    return null;
                }

                return user;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
