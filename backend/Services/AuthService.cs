using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using SmartSupport.Api.Data;
using SmartSupport.Api.DTOs;
using SmartSupport.Api.Models;

namespace SmartSupport.Api.Services;

public class AuthService : IAuthService
{
    private readonly MongoDbContext _db;
    private readonly IConfiguration _configuration;

    public AuthService(MongoDbContext db, IConfiguration configuration)
    {
        _db = db;
        _configuration = configuration;
    }

    public async Task<AuthResponse?> RegisterAsync(RegisterRequest request)
    {
        var email = request.Email.Trim().ToLowerInvariant();
        var existing = await _db.Users.Find(x => x.Email == email).FirstOrDefaultAsync();

        if (existing is not null)
            return null;

        var user = new User
        {
            Name = request.Name.Trim(),
            Email = email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            Role = "User"
        };

        await _db.Users.InsertOneAsync(user);
        return GenerateAuthResponse(user);
    }

    public async Task<AuthResponse?> LoginAsync(LoginRequest request)
    {
        var email = request.Email.Trim().ToLowerInvariant();
        var user = await _db.Users.Find(x => x.Email == email).FirstOrDefaultAsync();

        if (user is null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            return null;

        return GenerateAuthResponse(user);
    }

    public async Task<User?> GetUserByIdAsync(string userId)
    {
        return await _db.Users.Find(x => x.Id == userId).FirstOrDefaultAsync();
    }

    private AuthResponse GenerateAuthResponse(User user)
    {
        var key = _configuration["Jwt:Key"]
                  ?? throw new InvalidOperationException("Jwt:Key is missing.");

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var credentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(6),
            signingCredentials: credentials);

        return new AuthResponse(
            new JwtSecurityTokenHandler().WriteToken(token),
            user.Id,
            user.Name,
            user.Email,
            user.Role);
    }
}
