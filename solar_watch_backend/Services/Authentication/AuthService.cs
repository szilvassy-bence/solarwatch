using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using solar_watch_backend.Data;
using solar_watch_backend.Models;

namespace solar_watch_backend.Services.Authentication;

public class AuthService : IAuthService
{
    private readonly ITokenService _tokenService;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly ILogger<AuthService> _logger;
    private readonly SolarWatchContext _context;

    public AuthService(UserManager<IdentityUser> userManager, ITokenService tokenService, ILogger<AuthService> logger, SolarWatchContext context)
    {
        _userManager = userManager;
        _tokenService = tokenService;
        _logger = logger;
        _context = context;
    }

    public async Task<AuthResult> RegisterAsync(string email, string username, string password, string role)
    {
        try
        {
            var user = new IdentityUser { UserName = username, Email = email };
            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
            {
                _logger.LogError(1, password, "error during registration");
                return FailedRegistration(result, email, username);
            }

            await _userManager.AddToRoleAsync(user, role);
            var identityUser = await _userManager.FindByEmailAsync(email);
            _context.Users.Add(new User { IdentityUser = identityUser, Email = email, UserName = username });
            return new AuthResult(true, email, username, "");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<AuthResult> LoginAsync(string email, string password)
    {
        var managedUser = await _userManager.FindByEmailAsync(email);
        if (managedUser == null) return InvalidEmail(email);

        var isPasswordValid = await _userManager.CheckPasswordAsync(managedUser, password);
        if (!isPasswordValid) return InvalidPassword(email, managedUser.UserName);

        var roles = await _userManager.GetRolesAsync(managedUser);
        var accessToken = _tokenService.CreateToken(managedUser, roles[0]);
        return new AuthResult(true, managedUser.Email, managedUser.UserName, accessToken);
    }

    private AuthResult InvalidPassword(string email, string userName)
    {
        var result = new AuthResult(false, email, userName, "");
        result.ErrorMessages.Add("Bad credentials", "Invalid password");
        return result;
    }

    private AuthResult InvalidEmail(string email)
    {
        var result = new AuthResult(false, email, "", "");
        result.ErrorMessages.Add("Bad credentials", "Invalid email");
        return result;
    }

    private AuthResult FailedRegistration(IdentityResult result, string email, string username)
    {
        var authResult = new AuthResult(false, email, username, "");
        foreach (var error in result.Errors) authResult.ErrorMessages.Add(error.Code, error.Description);

        return authResult;
    }
}