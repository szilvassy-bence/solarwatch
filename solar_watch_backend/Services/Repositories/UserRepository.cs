using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using solar_watch_backend.Data;
using solar_watch_backend.Exceptions;
using solar_watch_backend.Models;
using solar_watch_backend.Models.Contracts;

namespace solar_watch_backend.Services.Repositories;

public class UserRepository : IUserRepository
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SolarWatchContext _context;

    public UserRepository(UserManager<IdentityUser> userManager, SolarWatchContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    public async Task<IEnumerable<ApplicationUser>> GetAll()
    {
        var users = await _context.ApplicationUsers
            .Include(u => u.IdentityUser)
            .Include(u => u.FavoriteCities)
            .ToListAsync();
        return users;
    }

    public async Task<ApplicationUser?> Get(string userName)
    {
        var user = await _context.ApplicationUsers
            .Include(u => u.IdentityUser)
            .Include(u => u.FavoriteCities)
            .FirstOrDefaultAsync(u => u.IdentityUser.UserName == userName);

        if (user is null)
        {
            throw new NullReferenceException("User not found.");
        }

        return user;
    }
    
    public async Task<UserDataChange> Update(string userName, UserDataChange userDataChange)
    {
        var user = await _context.ApplicationUsers
            .Include(au => au.IdentityUser)
            .FirstOrDefaultAsync(au => au.IdentityUser.UserName == userName);
        if (user is null)
        {
            throw (new NullReferenceException($"User not found."));
        }

        var userByNewEmail = await _userManager.FindByEmailAsync(userDataChange.Email);
        if (userByNewEmail != null && user.IdentityUser != userByNewEmail)
        {
            throw new ArgumentException("Email is already taken.");
        }

        var userByName = await _userManager.FindByNameAsync(userDataChange.UserName);
        if (userByName != null && user.IdentityUser != userByName)
        {
            throw new ArgumentException("User name is already taken.");
        }

        if (!string.IsNullOrEmpty(userDataChange.Email))
        {
            user.IdentityUser.Email = userDataChange.Email;
            user.IdentityUser.NormalizedEmail = userDataChange.Email.ToUpper();
        }

        if (!string.IsNullOrEmpty(userDataChange.UserName))
        {
            user.IdentityUser.UserName = userDataChange.UserName;
            user.IdentityUser.NormalizedUserName = userDataChange.UserName.ToUpper();
        }

        await _context.SaveChangesAsync();
        return userDataChange;
    }

    public async Task DeleteUser(string userName)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var user = await _context.ApplicationUsers
                .Include(au => au.IdentityUser)
                .FirstOrDefaultAsync(au => au.IdentityUser.UserName == userName);

            if (user == null)
            {
                throw new NullReferenceException("User not found.");
            }

            var identityUser = await _userManager.FindByIdAsync(user.IdentityUserId);

            if (identityUser == null)
            {
                throw new NullReferenceException("IdentityUser not found.");
            }

            _context.ApplicationUsers.Remove(user);
            await _context.SaveChangesAsync();

            var result = await _userManager.DeleteAsync(identityUser);
            if (!result.Succeeded)
            {
                throw new Exception("Failed to delete IdentityUser.");
            }

            await transaction.CommitAsync();
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<UserDataChange> UpdatePassword(string userName, UserPasswordChange userPasswordChange)
    {
        var user = await _userManager.FindByNameAsync(userName);
        
        if (user is null)
        {
            throw (new Exception());
        }
        
        var result = await _userManager.ChangePasswordAsync(user, userPasswordChange.ExistingPassword, userPasswordChange.NewPassword);
        if (!result.Succeeded)
        {
            var sb = new StringBuilder();
            
            foreach (var error in result.Errors)
            {
                sb.AppendLine($"Error code: {error.Code}, error description: {error.Description}");
            }
            throw (new Exception(sb.ToString()));
        }
        return new UserDataChange(user.Email, user.UserName);
    }

    public async Task<IEnumerable<City>> GetFavoriteCities(string userName)
    {
        var user = await _context.ApplicationUsers
            .Include(au => au.IdentityUser)
            .Include(au => au.FavoriteCities)
            .FirstOrDefaultAsync(au => au.IdentityUser.UserName == userName);
        
        if (user is null)
        {
            throw (new NullReferenceException());
        }
        
        return user.FavoriteCities;
    }
    
    public async Task<City> AddFavorite(string userName, int cityId)
    {
        var user = await _context.ApplicationUsers
            .Include(au => au.IdentityUser)
            .Include(au => au.FavoriteCities)
            .FirstOrDefaultAsync(au => au.IdentityUser.UserName == userName);

        if (user is null)
        {
            throw new NullReferenceException();
        }
        
        var city = await _context.Cities.FindAsync(cityId);
        
        if (city == null)
        {
            throw new Exception("City not found.");
        }
        
        if (user.FavoriteCities.Any(c => c.Id == cityId))
        {
            throw new CityAlreadyInFavoritesException("The city is already in the user's favorites.");
        }
        
        user.FavoriteCities.Add(city);
        await _context.SaveChangesAsync();

        return city;
    }

    public async Task DeleteFavorite(string userName, int cityId)
    {
        var user = await _context.ApplicationUsers
            .Include(au => au.IdentityUser)
            .Include(au => au.FavoriteCities)
            .FirstOrDefaultAsync(au => au.IdentityUser.UserName == userName);

        if (user is null)
        {
            throw new NullReferenceException();
        }

        var city = await _context.Cities.FindAsync(cityId);
        
        if (city == null)
        {
            throw new Exception("City not found.");
        }

        if (user.FavoriteCities.All(c => c.Id != city.Id))
        {
            throw new CityIsNotInFavoritesException("The city is not in the user's favorites");
        }

        user.FavoriteCities.Remove(city);
        await _context.SaveChangesAsync();
    }
}