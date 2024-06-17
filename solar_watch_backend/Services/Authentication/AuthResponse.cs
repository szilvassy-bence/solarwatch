namespace solar_watch_backend.Services.Authentication;

public record AuthResponse(string Email, string UserName, string Token);