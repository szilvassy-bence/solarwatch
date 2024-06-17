namespace solar_watch_backend.Services.Authentication;

public record AuthResult(bool Success, string Email, string UserName, string Token)
{
    // Error code - error message mapping
    public readonly Dictionary<string, string> ErrorMessages = new();
}