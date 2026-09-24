namespace SmartSupport.Api.DTOs;

public record AuthResponse(string Token, string UserId, string Name, string Email, string Role);
