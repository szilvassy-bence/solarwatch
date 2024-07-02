using Microsoft.AspNetCore.Identity;

namespace solar_watch_backend.Models;

public class ApplicationUser
{
    public int Id { get; init; }
    
    public string IdentityUserId { get; set; }
    public IdentityUser IdentityUser { get; set; }

    public ICollection<City> FavoriteCities { get; set; } = new List<City>();

}