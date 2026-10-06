using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EmployeeTaskApi.Data;
using EmployeeTaskApi.DTOs;
using EmployeeTaskApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace EmployeeTaskApi.Services;

public class AuthService : IAuthService
{
    private readonly AppDbContext _db;
    private readonly IPasswordHasher<AppUser> _hasher;
    private readonly IConfiguration _config;

    public AuthService(AppDbContext db, IPasswordHasher<AppUser> hasher, IConfiguration config)
    {
        _db = db; _hasher = hasher; _config = config;
    }

    public async Task<AuthResponse?> RegisterAsync(RegisterDto dto)
    {
        if (await _db.Users.AnyAsync(u => u.Username == dto.Username)) return null;

        var user = new AppUser { Username = dto.Username, Role = "User" };
        user.PasswordHash = _hasher.HashPassword(user, dto.Password);
        _db.Users.Add(user);
        await _db.SaveChangesAsync();
        return CreateToken(user);
    }

    public async Task<AuthResponse?> LoginAsync(LoginDto dto)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Username == dto.Username);
        if (user is null) return null;

        var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
        return result == PasswordVerificationResult.Failed ? null : CreateToken(user);
    }

    private AuthResponse CreateToken(AppUser user)
    {
        var jwt = _config.GetSection("Jwt");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.UtcNow.AddMinutes(double.Parse(jwt["ExpiryMinutes"] ?? "60"));

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var token = new JwtSecurityToken(jwt["Issuer"], jwt["Audience"], claims, expires: expires, signingCredentials: creds);
        return new AuthResponse(new JwtSecurityTokenHandler().WriteToken(token), user.Username, user.Role, expires);
    }
}
