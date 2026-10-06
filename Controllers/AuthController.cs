using EmployeeTaskApi.DTOs;
using EmployeeTaskApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeTaskApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[AllowAnonymous]
public class AuthController : ControllerBase
{
    private readonly IAuthService _auth;
    public AuthController(IAuthService auth) => _auth = auth;

    /// <summary>Register a new user (role: User).</summary>
    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> Register(RegisterDto dto)
    {
        var res = await _auth.RegisterAsync(dto);
        return res is null ? Conflict(new { message = "Username already taken." }) : Ok(res);
    }

    /// <summary>Login and get a JWT token.</summary>
    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login(LoginDto dto)
    {
        var res = await _auth.LoginAsync(dto);
        return res is null ? Unauthorized(new { message = "Invalid username or password." }) : Ok(res);
    }
}
