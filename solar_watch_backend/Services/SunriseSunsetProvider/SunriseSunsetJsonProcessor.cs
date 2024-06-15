using System;
using System.Globalization;
using System.Text.Json;
using solar_watch_backend.Models;

namespace solar_watch_backend.Services.SunriseSunsetProvider;

public class SunriseSunsetJsonProcessor : ISunriseSunsetJsonProcessor
{
    public SunriseSunset Process(string json, DateTime date)
    {
        if (string.IsNullOrEmpty(json))
        {
            throw new ArgumentException("Input json data cannot be empty.");
        }

        try
        {
            var jd = JsonDocument.Parse(json);
            var root = jd.RootElement;
            var result = root.GetProperty("results");
            var sunrise = result.GetProperty("sunrise");
            var sunset = result.GetProperty("sunset");
            Console.WriteLine(sunset.ToString());
            var sunriseDate = ConvertToDateTime(sunrise.ToString(), date);
            var sunsetDate = ConvertToDateTime(sunset.ToString(), date);
            return new SunriseSunset { Sunrise = sunriseDate, Sunset = sunsetDate };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private DateTime ConvertToDateTime(string time, DateTime date)
    {
        var format = "h:mm:ss tt";

        var combinedDateTimeString = $"{date.ToString("MM/dd/yyyy")} {time}";

        DateTime result;
        if (DateTime.TryParseExact(combinedDateTimeString, "MM/dd/yyyy " + format, null,
                DateTimeStyles.None, out result))
            return result;
        throw new FormatException("Failed to parse the time string.");
    }
}