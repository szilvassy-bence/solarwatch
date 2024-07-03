using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using solar_watch_backend.Exceptions;
using solar_watch_backend.Models;
using solar_watch_backend.Models.Contracts;
using solar_watch_backend.Services.Repositories;

namespace solar_watch_backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<UserController> _logger;

    public UserController(IUserRepository userRepository, ILogger<UserController> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }

    [HttpGet("get-all"), Authorize(Roles = "Admin")]
    public async Task<ActionResult<IEnumerable<ApplicationUser>>> GetAll()
    {
        try
        {
            return Ok(await _userRepository.GetAll());
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, "Could not process the request.");
        }
    }

    [HttpGet("get"), Authorize(Roles = "ApplicationUser, Admin")]
    public async Task<ActionResult<ApplicationUser?>> Get()
    {
        try
        {
            var userName = User.FindFirst(ClaimTypes.Name)?.Value;
            Console.WriteLine(userName);
            return Ok(await _userRepository.Get(userName));
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpPatch("update"), Authorize(Roles = "ApplicationUser, Admin")]
    public async Task<ActionResult<UserDataChange>> Update([FromBody] UserDataChange userDataChange)
    {
        
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var userName = User.FindFirst(ClaimTypes.Name)?.Value;
            return Ok(await _userRepository.Update(userName, userDataChange));
        }
        
        catch (Exception e)
        {
            return BadRequest(e);
        }
    }

    [HttpDelete("delete-user"), Authorize(Roles = "ApplicationUser, Admin")]
    public async Task<IActionResult> DeleteUser()
    {
        try
        {
            var userName = User.FindFirst(ClaimTypes.Name)?.Value;
            _logger.LogInformation("Retrieved userName: {userName}", userName);
            if (userName == null) return NotFound();
            await _userRepository.DeleteUser(userName);
            return NoContent();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return NotFound();
        }
    }

    [HttpPatch("update-password"), Authorize(Roles = "ApplicationUser, Admin")]
    public async Task<ActionResult<UserDataChange>> UpdatePassword(UserPasswordChange userPasswordChange)
    {
        try
        {
            var userName = User.FindFirst(ClaimTypes.Name)?.Value;
            return Ok(await _userRepository.UpdatePassword(userName, userPasswordChange));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return NotFound(e.Message);
        }
    }

    [HttpGet("favorites"), Authorize(Roles = "ApplicationUser, Admin")]
    public async Task<ActionResult<IEnumerable<City>>> GetFavorites()
    {
        try
        {
            var userName = User.FindFirst(ClaimTypes.Name)?.Value;
            return Ok(await _userRepository.GetFavoriteCities(userName));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return NotFound(e.Message);
        }
    }
    
    
    [HttpPost("{cityId}/add-city"), Authorize(Roles = "ApplicationUser, Admin")]
    public async Task<ActionResult> AddFavorite(int cityId)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var userName = User.FindFirst(ClaimTypes.Name)?.Value;
            var city = await _userRepository.AddFavorite(userName, cityId);
            return Ok(city);
        }
        catch (CityAlreadyInFavoritesException e)
        {
            return BadRequest(new { message = e.Message });
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return NotFound(e.Message);
        }
    }

    [HttpDelete("{cityId}/delete-city"), Authorize(Roles = "ApplicationUser, Admin")]
    public async Task<ActionResult> DeleteCity(int cityId)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var userName = User.FindFirst(ClaimTypes.Name)?.Value;
            await _userRepository.DeleteFavorite(userName, cityId);
            return NoContent();
        }
        catch (CityIsNotInFavoritesException e)
        {
            return BadRequest(new { message = e.Message });
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return NotFound(e.Message);
        }
    }
}