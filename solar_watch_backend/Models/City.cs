using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace solar_watch_backend.Models;

public class City
{
    public int Id { get; set; }
    
    public string Name { get; set; }

    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string Country { get; set; }

    [StringLength(50)] public string? State { get; set; }

    public ICollection<SunriseSunset> SunriseSunsets { get; set; } = new List<SunriseSunset>();

    public override string ToString()
    {
        return $"{nameof(Name)}: {Name}, {nameof(Country)}: {Country}";
    }
}