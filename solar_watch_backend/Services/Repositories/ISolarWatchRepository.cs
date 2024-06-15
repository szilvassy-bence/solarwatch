using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using solar_watch_backend.Models;

namespace solar_watch_backend.Services.Repositories;

public interface ISolarWatchRepository
{
    Task<IEnumerable<City>> GetAllCities();
    Task<City> GetCityByName(string city);
    Task<IEnumerable<SunriseSunset>> GetAllSunriseSunsets();
    Task<SunriseSunset> GetSunriseSunsetByCityByDate(string city, DateTime date);
    Task<City?> GetCityById(int id);
    Task<IEnumerable<City>> SearchCities(string searchTerm);
    Task DeleteCity(City city);
    Task<SunriseSunset> GetSunriseSunsetById(int id);
    Task DeleteSunriseSunset(SunriseSunset sunriseSunset);
}