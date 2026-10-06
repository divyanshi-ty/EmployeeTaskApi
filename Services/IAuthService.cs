using EmployeeTaskApi.DTOs;

namespace EmployeeTaskApi.Services;

public interface IAuthService
{
    Task<AuthResponse?> RegisterAsync(RegisterDto dto);   // null => username taken
    Task<AuthResponse?> LoginAsync(LoginDto dto);         // null => invalid credentials
}
