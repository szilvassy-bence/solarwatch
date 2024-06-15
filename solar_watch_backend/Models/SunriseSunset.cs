using System;
using System.ComponentModel.DataAnnotations;

namespace solar_watch_backend.Models;

public class SunriseSunset
{
    public int Id { get; set; }
    
    public DateTime Sunrise { get; set; }

    public DateTime Sunset { get; set; }

    public DateOnly Day => DateOnly.FromDateTime(Sunrise);

    // called foreign key property
    public int CityId { get; set; }

    public override string ToString()
    {
        return
            $"{nameof(Id)}: {Id}, {nameof(Sunrise)}: {Sunrise}, {nameof(Sunset)}: {Sunset}, {nameof(CityId)}: {CityId}";
    }
}