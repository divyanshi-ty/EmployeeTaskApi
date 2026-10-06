using System.ComponentModel.DataAnnotations;

namespace EmployeeTaskApi.DTOs;

public class RegisterDto
{
    [Required, MinLength(3), MaxLength(50)] public string Username { get; set; } = string.Empty;
    [Required, MinLength(6)] public string Password { get; set; } = string.Empty;
}

public class LoginDto
{
    [Required] public string Username { get; set; } = string.Empty;
    [Required] public string Password { get; set; } = string.Empty;
}

public record AuthResponse(string Token, string Username, string Role, DateTime ExpiresAt);
