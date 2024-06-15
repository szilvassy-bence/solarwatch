using System;
using System.Net.Http;
using System.Threading.Tasks;
using solar_watch_backend.Models;

namespace solar_watch_backend.Services.SunriseSunsetProvider;

public class SunriseSunsetDataProvider : ISunriseSunsetDataProvider
{
    public async Task<string> GetSunriseSunsetData(City city, DateTime date)
    {
        var url =
            $"https://api.sunrise-sunset.org/json?lat={city.Latitude}&lng={city.Longitude}&date={date.ToString("yyyy-MM-dd")}";
        Console.WriteLine(url);
        using var client = new HttpClient();
        var sunInfoData = await client.GetStringAsync(url);
        return sunInfoData;
    }
    
}