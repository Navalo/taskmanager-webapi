using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.Services;
using TaskManager.Domain.DTOs;
using System;

namespace TaskManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        private readonly IAuthService _authService = authService;

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
        {
            if (request == null)
                return BadRequest(new ApiResponse<string>(false, "Request body is null."));

            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse<object>(false, "Validation failed.", ModelState));

            try
            {
                var result = await _authService.RegisterAsync(request);
                if (!result)
                    return Conflict(new ApiResponse<string>(false, "Username already exists."));

                return Ok(new ApiResponse<string>(true, "User registered successfully."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<string>(false, "An error occurred while processing your request.", ex.Message));
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            if (request == null)
                return BadRequest(new ApiResponse<string>(false, "Request body is null."));

            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse<object>(false, "Validation failed.", ModelState));

            try
            {
                var result = await _authService.SignInAsync(request);
                if (result == null)
                    return Unauthorized(new ApiResponse<UserDto?>(false, "Invalid credentials.",null));

                var userDto = new UserDto 
                {
                    Id = result.Id,
                    Username = result.Username 
                };
                return Ok(new ApiResponse<UserDto>(true, "Login successful.", userDto));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<UserDto?>(false, ex.Message, null));
            }
        }
    }
}
