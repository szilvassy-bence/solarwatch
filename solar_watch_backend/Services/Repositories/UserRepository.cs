using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using solar_watch_backend.Data;
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

    public async Task<IEnumerable<User>> GetAll()
    {
        try
        {
            var users = await _context.Users.Include(u => u.IdentityUser).ToListAsync();
            return users;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public Task<IdentityUser> GetById()
    {
        throw new NotImplementedException();
    }

    public async Task<IdentityUser?> GetByName(string userName)
    {
        try
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user is null)
            {
                throw (new NullReferenceException($"User with name {userName} is not registered."));
            }

            return user;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<IdentityUser> GetByEmail(string email)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
            {
                throw (new NullReferenceException($"User with name {email} is not registered."));
            }

            return user;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<UserDataChange> UpdateById(string id, UserDataChange userDataChange)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user is null)
            {
                throw (new NullReferenceException($"User with id {id} not found."));
            }

            var userByEmail = await _userManager.FindByEmailAsync(userDataChange.Email);
            if (userByEmail != null && user != userByEmail)
            {
                throw (new Exception($"Email: {userDataChange.Email} is already taken!"));
            }

            var userByName = await _userManager.FindByNameAsync(userDataChange.UserName);
            if (userByName != null && userByName != user)
            {
                throw (new Exception($"User name: {userDataChange.UserName} is already taken!"));
            }

            if (!string.IsNullOrEmpty(userDataChange.Email))
            {
                user.Email = userDataChange.Email;
                user.NormalizedEmail = userDataChange.Email.ToUpper();
            }

            if (!string.IsNullOrEmpty(userDataChange.UserName))
            {
                user.UserName = userDataChange.UserName;
                user.NormalizedUserName = userDataChange.UserName.ToUpper();
            }

            await _context.SaveChangesAsync();
            return userDataChange;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task DeleteUserById(string id)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user is null)
            {
                throw (new NullReferenceException());
            }

            await _userManager.DeleteAsync(user);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<UserDataChange> UpdatePasswordById(string id, UserPasswordChange userPasswordChange)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(id);
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
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
    }
}