using Microsoft.AspNetCore.Identity;

namespace solar_watch_backend.Services.Authentication;

public class AuthSeeder
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly ILogger<AuthSeeder> _logger;

    public AuthSeeder(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager, ILogger<AuthSeeder> logger)
    {
        _roleManager = roleManager;
        _userManager = userManager;
        _logger = logger;
    }

    public void AddRoles()
    {
        try
        {
            var tAdmin = CreateAdminRole(_roleManager);
            tAdmin.Wait();
            var tUser = CreateUserRole(_roleManager);
            tUser.Wait();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            _logger.LogError("error happened: " + e);
            throw;
        }
    }

    private async Task CreateUserRole(RoleManager<IdentityRole> roleManager)
    {
        if (!await roleManager.RoleExistsAsync("User"))
        {
            await roleManager.CreateAsync(new IdentityRole("User"));
        }
    }

    private async Task CreateAdminRole(RoleManager<IdentityRole> roleManager)
    {
        if (!await roleManager.RoleExistsAsync("Admin"))
        {
            await roleManager.CreateAsync(new IdentityRole("Admin"));
        }
    }

    public void AddAdmin()
    {
        var tAdmin = CreateAdminIfNotExists();
        tAdmin.Wait();
    }

    private async Task CreateAdminIfNotExists()
    {
        var adminInDb = await _userManager.FindByEmailAsync("a@a");
        if (adminInDb == null)
        {
            var admin = new IdentityUser { UserName = "admin", Email = "a@a" };
            var adminCreated = await _userManager.CreateAsync(admin, "admin123");
            if (adminCreated.Succeeded) await _userManager.AddToRoleAsync(admin, "Admin");
        }
    }
}