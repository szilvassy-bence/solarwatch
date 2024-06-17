using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using solar_watch_backend.Models;

namespace solar_watch_backend.Data;

public class SolarWatchContext : IdentityDbContext<IdentityUser, IdentityRole, string>
{
    public SolarWatchContext(DbContextOptions<SolarWatchContext> options) : base(options)
    {
    }

    public DbSet<City> Cities { get; set; }
    public DbSet<SunriseSunset> SunriseSunsets { get; set; }
    public DbSet<User> Users { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<City>()
            .HasIndex(c => c.Name)
            .IsUnique();
        modelBuilder.Entity<City>()
            .HasData(
                new City
                {
                    Id = 1, Name = "London", Latitude = 51.509865, Longitude = -0.118092, Country = "GB",
                    State = "England"
                },
                new City
                {
                    Id = 2, Name = "Budapest", Latitude = 47.497913, Longitude = 19.040236, Country = "HU"
                },
                new City
                {
                    Id = 3, Name = "Paris", Latitude = 48.864716, Longitude = 2.349014, Country = "FR",
                    State = "Ile-de-France"
                }
            );

        //modelBuilder.Entity<City>().HasMany<SunInfo>().WithMany();
    }
}