using System.Net.Http;
using System.Threading.Tasks;

namespace solar_watch_backend.Services.LatLngProvider;

public class LatLngProvider : ILatLngProvider
{
    public async Task<string> GetLatLng(string city)
    {
        try
        {
            var url = $"http://api.openweathermap.org/geo/1.0/direct?q={city}&appid={Environment.GetEnvironmentVariable("OPEN_WEATHER_API_KEY")}";

            using var client = new HttpClient();
            var response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var latLngData = await response.Content.ReadAsStringAsync();

                if (latLngData.Trim() == "[]")
                {
                    throw new Exception("No city found for the given query.");
                }

                return latLngData;
            }
            else
            {
                throw new Exception($"Failed to retrieve data. Status code: {response.StatusCode}");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}