using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using solar_watch_backend.Models;
using solar_watch_backend.Models.Contracts;
using solar_watch_backend.Services.Repositories;

namespace solar_watch_backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserRepository _userRepository;

    public UserController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    [HttpGet("GetAll")]
    public async Task<ActionResult<IEnumerable<User>>> GetAll()
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

    [HttpGet("/name/{userName}")]
    public async Task<ActionResult<User?>> GetUserByName(string userName)
    {
        try
        {
            return Ok(await _userRepository.GetByName(userName));
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpGet("/email/{email}")]
    public async Task<ActionResult<IdentityUser>> GetUserByEmail(string email)
    {
        try
        {
            Console.WriteLine(email);
            return Ok(await _userRepository.GetByEmail(email));
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpPatch("/update/{id}")]
    public async Task<ActionResult<UserDataChange>> UpdateById(string id, [FromBody] UserDataChange userDataChange)
    {
        try
        {
            return Ok(await _userRepository.UpdateById(id, userDataChange));
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpDelete("/delete/{id}")]
    public async Task<IActionResult> DeleteById(string id)
    {
        try
        {
            await _userRepository.DeleteUserById(id);
            return NoContent();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return NotFound();
        }
    }

    [HttpPatch("/update-password/{id}")]
    public async Task<ActionResult<UserDataChange>> UpdatePasswordById(string id, [FromBody] UserPasswordChange userPasswordChange)
    {
        try
        {
            return Ok(await _userRepository.UpdatePasswordById(id, userPasswordChange));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return NotFound(e.Message);
        }
    }
    
}