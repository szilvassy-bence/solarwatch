

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using solar_watch_backend.Models;
using solar_watch_backend.Services.Repositories;

namespace solar_watch_backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SolarWatchController : ControllerBase
{
    private readonly ILogger<SolarWatchController> _logger;
    private readonly ISolarWatchRepository _repository;

    public SolarWatchController(ILogger<SolarWatchController> logger, ISolarWatchRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    [HttpGet("GetAllCities")]
    public async Task<ActionResult<IEnumerable<City>>> GetAllCities()
    {
        return Ok(await _repository.GetAllCities());
    }

    [HttpGet("cities/{city}")]
    public async Task<ActionResult<City>> GetCity(string city)
    {
        try
        {
            var cityToReturn = await _repository.GetCityByName(city);
            return Ok(cityToReturn);
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }
    }

    
    /*public async Task<ActionResult<IEnumerable<City>>> SearchCities(string searchTerm)
    {
        var cities = await _repository.SearchCities(searchTerm);
        return Ok(cities);
    }*/
    
    
    [HttpDelete("cities/{id}/delete")]
    public async Task<ActionResult<City>> DeleteCity(int id)
    {
        var city = await _repository.GetCityById(id);
        if (city == null)
        {
            return NotFound("The city is not found.");
        }

        await _repository.DeleteCity(city);
        return NoContent();
    }

    
    [HttpGet("sunrisesunsets")]
    public async Task<ActionResult<IEnumerable<City>>> GetAllSunInfos()
    {
        return Ok(await _repository.GetAllSunriseSunsets());
    }

    [HttpGet("sunrisesunsets/{city}/{date}")]
    public async Task<ActionResult<SunriseSunset>> GetSunInfoByCityByDate(string city, DateTime date)
    {
        try
        {
            return Ok(await _repository.GetSunriseSunsetByCityByDate(city, date));
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpDelete("sunrisesunsets/{id}/Delete")]
    public async Task<IActionResult> DeleteSunInfoById(int id)
    {
        var sunInfo = await _repository.GetSunriseSunsetById(id);
        if (sunInfo == null)
        {
            return NotFound("The suninfo is not found.");
        }

        await _repository.DeleteSunriseSunset(sunInfo);
        return NoContent();
    }
}