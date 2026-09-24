using SmartSupport.Api.DTOs;
using SmartSupport.Api.Models;

namespace SmartSupport.Api.Services;

public interface IAuthService
{
    Task<AuthResponse?> RegisterAsync(RegisterRequest request);
    Task<AuthResponse?> LoginAsync(LoginRequest request);
    Task<User?> GetUserByIdAsync(string userId);
}
