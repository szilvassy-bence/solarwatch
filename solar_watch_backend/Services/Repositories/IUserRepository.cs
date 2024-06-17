using System.Collections.Generic;
using System.Threading.Tasks;
using solar_watch_backend.Models;

namespace solar_watch_backend.Services.Repositories;
using Microsoft.AspNetCore.Identity;
using solar_watch_backend.Models.Contracts;


public interface IUserRepository
{
    Task<IEnumerable<User>> GetAll();
    Task<IdentityUser> GetById();
    Task<IdentityUser> GetByName(string userName);
    Task<IdentityUser> GetByEmail(string email);
    Task<UserDataChange> UpdateById(string id, UserDataChange userDataChange);
    Task DeleteUserById(string id);
    Task<UserDataChange> UpdatePasswordById(string id, UserPasswordChange userPasswordChange);
}