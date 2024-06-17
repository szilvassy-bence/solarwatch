using Microsoft.AspNetCore.Identity;

namespace solar_watch_backend.Services.Authentication;

public interface ITokenService
{
    public string CreateToken(IdentityUser user, string role);
}