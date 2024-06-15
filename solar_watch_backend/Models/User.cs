using Microsoft.AspNetCore.Identity;

namespace solar_watch_backend.Models;

public class User
{
    public int Id { get; init; }
    
    public IdentityUser IdentityUser { get; set; }
    
    public string Email { get; set; }
    
    public string UserName { get; set; }
}