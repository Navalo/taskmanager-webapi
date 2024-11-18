using System.ComponentModel.DataAnnotations;

namespace TaskManager.Domain.DTOs
{
    public class RegisterRequestDto
    {
        [Required]
        public required string Username { get; set; }
        [Required]
        public required string Password { get; set; }
    }
}
