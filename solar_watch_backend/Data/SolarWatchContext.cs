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
    public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<City>()
            .HasIndex(c => c.Name)
            .IsUnique();
        
        modelBuilder.Entity<City>()
            .HasMany(c => c.SunriseSunsets)
            .WithOne(ss => ss.City)
            .HasForeignKey(ss => ss.CityId);
        
        modelBuilder.Entity<ApplicationUser>()
            .HasMany(u => u.FavoriteCities)
            .WithMany();

        modelBuilder.Entity<ApplicationUser>()
            .HasOne(au => au.IdentityUser)
            .WithOne()
            .HasForeignKey<ApplicationUser>(au => au.IdentityUserId)
            .OnDelete(DeleteBehavior.Cascade);
        
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
        
        modelBuilder.Entity<SunriseSunset>()
            .HasData(
                new SunriseSunset
                {
                    Id = 1,
                    Sunrise = new DateTime(2023, 6, 21, 4, 43, 0),
                    Sunset = new DateTime(2023, 6, 21, 21, 21, 0),
                    CityId = 1
                },
                new SunriseSunset
                {
                    Id = 2,
                    Sunrise = new DateTime(2023, 6, 21, 5, 0, 0),
                    Sunset = new DateTime(2023, 6, 21, 20, 50, 0),
                    CityId = 2
                }
            );
    }
}