using System;
using System.Threading.Tasks;
using solar_watch_backend.Models;

namespace solar_watch_backend.Services.SunriseSunsetProvider;

public interface ISunriseSunsetDataProvider
{
    Task<string> GetSunriseSunsetData(City city, DateTime date);
}