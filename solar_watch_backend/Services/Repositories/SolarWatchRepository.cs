using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using solar_watch_backend.Data;
using solar_watch_backend.Models;
using solar_watch_backend.Services.LatLngProvider;
using solar_watch_backend.Services.SunriseSunsetProvider;

namespace solar_watch_backend.Services.Repositories;

public class SolarWatchRepository : ISolarWatchRepository
{
    private readonly SolarWatchContext _context;
    private readonly ILatLngJsonProcessor _latLngProcessor;
    private readonly ILatLngProvider _latLngProvider;
    private readonly ISunriseSunsetJsonProcessor _sunriseSunsetJsonProcessor;
    private readonly ISunriseSunsetDataProvider _sunriseSunsetProvider;


    public SolarWatchRepository(ILatLngProvider provider, ILatLngJsonProcessor processor, SolarWatchContext context,
        ISunriseSunsetJsonProcessor sunriseSunsetJsonProcessor, ISunriseSunsetDataProvider sunriseSunsetProvider)
    {
        _latLngProvider = provider;
        _latLngProcessor = processor;
        _context = context;
        _sunriseSunsetJsonProcessor = sunriseSunsetJsonProcessor;
        _sunriseSunsetProvider = sunriseSunsetProvider;
    }

    public async Task<IEnumerable<City>> GetAllCities()
    {
        return await _context.Cities
            .Include(x => x.SunriseSunsets)
            .ToListAsync();
    }

    public async Task<City> GetCityByName(string city)
    {
        var dbCity = await _context.Cities
            .Include(c => c.SunriseSunsets)
            .FirstOrDefaultAsync(c => c.Name == city);
        if (dbCity is null)
        {
            try
            {
                var json = await _latLngProvider.GetLatLng(city);
                var cityEntry = _context.Cities.Add(_latLngProcessor.Process(json));
                await _context.SaveChangesAsync();
                return cityEntry.Entity;
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        return dbCity;
    }

    public async Task<City?> GetCityById(int id)
    {
        return await _context.Cities
            .Include(x => x.SunriseSunsets)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IEnumerable<City>> SearchCities(string searchTerm)
    {
        return await _context.Cities
            .Where(x => x.Name.Contains(searchTerm))
            .ToListAsync();
    }

    public async Task<IEnumerable<SunriseSunset>> GetAllSunriseSunsets()
    {
        return await _context.SunriseSunsets.ToListAsync();
    }

    public async Task<SunriseSunset> GetSunriseSunsetByCityByDate(string city, DateTime date)
    {
        try
        {
            var dbCity = await GetCityByName(city);

            if (date == new DateTime()) date = DateTime.Now;

            // check if city already has sun information for the required date and return
            if (dbCity.SunriseSunsets != null && dbCity.SunriseSunsets.Count > 0)
            {
                var sunInfoOnDate = dbCity.SunriseSunsets.FirstOrDefault(x => x.Day == DateOnly.FromDateTime(date));
                if (sunInfoOnDate != null) return sunInfoOnDate;
            }

            // fetch the new sun information for the new date
            var json = await _sunriseSunsetProvider.GetSunriseSunsetData(dbCity, date);
            var suninfo = _sunriseSunsetJsonProcessor.Process(json, date);
            suninfo.CityId = dbCity.Id;
            var sunInfoEntry = _context.SunriseSunsets.Add(suninfo);
            await _context.SaveChangesAsync();
            return sunInfoEntry.Entity;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task DeleteCity(City city)
    {
        _context.Cities.Remove(city);
        await _context.SaveChangesAsync();
    }

    public async Task<SunriseSunset> GetSunriseSunsetById(int id)
    {
        return await _context.SunriseSunsets.FindAsync(id);
    }

    public async Task DeleteSunriseSunset(SunriseSunset sunriseSunset)
    {
        _context.SunriseSunsets.Remove(sunriseSunset);
        await _context.SaveChangesAsync();
    }
}