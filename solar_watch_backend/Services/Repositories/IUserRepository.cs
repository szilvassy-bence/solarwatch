using System.Collections.Generic;
using System.Threading.Tasks;
using solar_watch_backend.Models;

namespace solar_watch_backend.Services.Repositories;
using Microsoft.AspNetCore.Identity;
using solar_watch_backend.Models.Contracts;


public interface IUserRepository
{
    Task<IEnumerable<ApplicationUser>> GetAll();
    Task<ApplicationUser?> Get(string userName);
    Task<UserDataChange> Update(string userName, UserDataChange userDataChange);
    Task DeleteUser(string userName);
    Task<UserDataChange> UpdatePassword(string userName, UserPasswordChange userPasswordChange);
    Task<IEnumerable<City>> GetFavoriteCities(string userName);
    Task<City> AddFavorite(string userName, int cityId);
    Task DeleteFavorite(string userName, int cityId);
}