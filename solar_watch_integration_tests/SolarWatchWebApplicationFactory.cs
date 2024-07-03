using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using solar_watch_backend.Data;
using solar_watch_backend.Services.LatLngProvider;
using solar_watch_backend.Services.SunriseSunsetProvider;
using solar_watch_integration_tests.helpers;

namespace solar_watch_integration_tests;

public class SolarWatchWebApplicationFactory : WebApplicationFactory<Program>
{
    // the original config runs before this
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            var dbContextDescriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                     typeof(DbContextOptions<SolarWatchContext>));

            if (dbContextDescriptor != null) services.Remove(dbContextDescriptor);

            var serviceProvider = new ServiceCollection().AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

            services.AddDbContext<SolarWatchContext>(options =>
            {
                options.UseInMemoryDatabase("InMemoryTestDb");
                options.UseInternalServiceProvider(serviceProvider);
            });
            
            services.AddTransient<Seeder>();

            var sp = services.BuildServiceProvider();

            using var scope = sp.CreateScope();
            
            using var solarWatchContext = scope.ServiceProvider.GetRequiredService<SolarWatchContext>();
            solarWatchContext.Database.EnsureDeleted();
            solarWatchContext.Database.EnsureCreated();
            
            var seeder = scope.ServiceProvider.GetRequiredService<Seeder>();
            seeder.ReinitializeSolarWatchDbForTests();
            seeder.ReinitializeIdentityUserDbForTests();
        });

        builder.UseEnvironment("Development");
    }
}