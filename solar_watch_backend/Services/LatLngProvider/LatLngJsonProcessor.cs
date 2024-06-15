using System;
using System.Text.Json;
using solar_watch_backend.Models;

namespace solar_watch_backend.Services.LatLngProvider;

public class LatLngJsonProcessor : ILatLngJsonProcessor
{
    public City Process(string json)
    {
        if (string.IsNullOrEmpty(json))
        {
            throw new ArgumentException("Input json data cannot be empty.");
        }
        try
        {
            var jd = JsonDocument.Parse(json);
            var root = jd.RootElement[0];
            var lat = root.GetProperty("lat");
            var lon = root.GetProperty("lon");
            var name = root.GetProperty("name");
            var country = root.GetProperty("country");
            City city;
            JsonElement state;
            if (root.TryGetProperty("state", out state))
                city = new City
                {
                    Latitude = lat.GetDouble(), 
                    Longitude = lon.GetDouble(), 
                    Name = name.GetString(),
                    Country = country.GetString(), 
                    State = state.GetString()
                };
            else
                city = new City
                {
                    Latitude = lat.GetDouble(), 
                    Longitude = lon.GetDouble(), 
                    Name = name.GetString(),
                    Country = country.GetString()
                };

            return city;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}