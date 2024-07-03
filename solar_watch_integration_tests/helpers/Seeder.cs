using Xunit.Abstractions;

namespace solar_watch_integration_tests.helpers;
using solar_watch_backend.Data;
using solar_watch_backend.Models;
using Microsoft.AspNetCore.Identity;

public class Seeder
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SolarWatchContext _solarWatchContext;

    public Seeder(
        RoleManager<IdentityRole> roleManager, 
        UserManager<IdentityUser> userManager,
        SolarWatchContext solarWatchContext)
    {
        _roleManager = roleManager;
        _userManager = userManager;
        _solarWatchContext = solarWatchContext;
    }

    public void InitializeSolarWatchForTests()
    {
        _solarWatchContext.Cities.AddRange(GetSeedingCities());
        _solarWatchContext.SunriseSunsets.AddRange(GetSeedingSunriseSunsets());
        _solarWatchContext.SaveChanges();
    }

    public void ReinitializeSolarWatchDbForTests()
    {
        _solarWatchContext.Cities.RemoveRange(_solarWatchContext.Cities);
        _solarWatchContext.SunriseSunsets.RemoveRange(_solarWatchContext.SunriseSunsets);
        InitializeSolarWatchForTests();
    }

    public List<City> GetSeedingCities()
    {
        return new List<City>()
        {
            new City() { Id = 1, Country = "HU", Latitude = 100, Longitude = 100, Name = "Miskolc" },
            new City() { Id = 2, Country = "HU", Latitude = 123, Longitude = 123, Name = "Debrecen" },
            new City() { Id = 3, Country = "HU", Latitude = 200, Longitude = 200, Name = "Budapest" },
        };
    }

    public List<SunriseSunset> GetSeedingSunriseSunsets()
    {
        return new List<SunriseSunset>()
        {
            new SunriseSunset() { Id = 1, CityId = 1, Sunrise = new DateTime(2024, 01, 01), Sunset = new DateTime(2024, 01, 01) },
            new SunriseSunset() { Id = 2, CityId = 2, Sunrise = new DateTime(2024, 02, 01), Sunset = new DateTime(2024, 02, 01) },
            new SunriseSunset() { Id = 3, CityId = 3, Sunrise = new DateTime(2024, 03, 01), Sunset = new DateTime(2024, 03, 01) },
        };
    }

    public void ReinitializeIdentityUserDbForTests()
    {
        _solarWatchContext.Users.RemoveRange(_solarWatchContext.Users);
        _solarWatchContext.Roles.RemoveRange(_solarWatchContext.Roles);
        InitializeIdentityUserDbForTests();
    }

    public async Task InitializeIdentityUserDbForTests()
    {
        var roles = AddRoles();
        var users = AddUsers();
        for (int i = 0; i < roles.Count; i++)
        {
            await _roleManager.CreateAsync(roles[i]);
            var identityResult = await _userManager.CreateAsync(users[0], $"password{i + 1}");
            if (identityResult.Succeeded) await _userManager.AddToRoleAsync(users[i], roles[i].Name);
        }

        ApplicationUser applicationUser = new ApplicationUser
        {
            IdentityUser = users[1]
        };
    }

    public List<IdentityRole> AddRoles()
    {
        return new List<IdentityRole>()
        {
            new IdentityRole("Admin"),
            new IdentityRole("ApplicationUser"),
        };
    }

    public List<IdentityUser> AddUsers()
    {
        return new List<IdentityUser>()
        {
            new IdentityUser() { UserName = "admin", Email = "admin@user" },
            new IdentityUser() { UserName = "user", Email = "user@user" },
        };
    }

}