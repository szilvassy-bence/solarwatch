// solar watch: https://sunrise-sunset.org/api
// openweather: https://openweathermap.org/api/geocoding-api

using System.Text;
using System.Text.Json.Serialization;
//using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using solar_watch_backend.Data;
using solar_watch_backend.Services;
using solar_watch_backend.Services.LatLngProvider;
using solar_watch_backend.Services.Repositories;
using solar_watch_backend.Services.SunriseSunsetProvider;

var builder = WebApplication.CreateBuilder(args);

AddEnvironmentVariables();
AddServices();
AddDbContext();
//AddIdentity();
ConfigureSwagger();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<SolarWatchContext>();
    context.Database.EnsureCreated();
}

using (var dbScope = app.Services.CreateScope())
{
    var dbContext = dbScope.ServiceProvider.GetRequiredService<SolarWatchContext>();
    if (dbContext.Database.GetPendingMigrations().Any())
    {
        dbContext.Database.Migrate();
    }
}

/*
using var scope = app.Services.CreateScope();
var authenticationSeeder = scope.ServiceProvider.GetRequiredService<AuthenticationSeeder>();
authenticationSeeder.AddRoles();
authenticationSeeder.AddAdmin();*/

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

/*app.UseAuthentication();
app.UseAuthorization();*/

app.MapControllers();

app.Run();


void AddServices()
{
    builder.Services.AddControllers()
        .AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddScoped<ILatLngProvider, LatLngProvider>();
    builder.Services.AddScoped<ILatLngJsonProcessor, LatLngJsonProcessor>();
    builder.Services.AddScoped<ISolarWatchRepository, SolarWatchRepository>();
    // builder.Services.AddScoped<IUserRepository, UserRepository>();
    builder.Services.AddScoped<ISunriseSunsetJsonProcessor, SunriseSunsetJsonProcessor>();
    builder.Services.AddScoped<ISunriseSunsetDataProvider, SunriseSunsetDataProvider>();
    
}

void ConfigureSwagger()
{
    builder.Services.AddSwaggerGen(option =>
    {
        option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
        option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please enter a valid token",
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            BearerFormat = "JWT",
            Scheme = "Bearer"
        });
        option.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new string[] { }
            }
        });
    });
}

void AddDbContext()
{
    /*var conStrBuilder = new SqlConnectionStringBuilder(builder.Configuration.GetConnectionString("SolarWatch"));
    conStrBuilder.Password = Environment.GetEnvironmentVariable("SQL_PASSWORD");
    conStrBuilder.DataSource = Environment.GetEnvironmentVariable("SW_DB_HOST") ?? "localhost,1433";*/
    
    var connectionString = builder.Configuration.GetConnectionString("SolarWatch");
    var sqlPassword = Environment.GetEnvironmentVariable("SQL_PASSWORD");
    var sqlServerHost = Environment.GetEnvironmentVariable("SW_DB_HOST") ?? "localhost,1433";

    if (string.IsNullOrEmpty(connectionString))
    {
        throw new InvalidOperationException("Connection string 'SolarWatch' not found in configuration.");
    }

    if (string.IsNullOrEmpty(sqlPassword))
    {
        throw new InvalidOperationException("Environment variable 'SQL_PASSWORD' not set.");
    }

    if (string.IsNullOrEmpty(sqlServerHost))
    {
        throw new InvalidOperationException("Environment variable 'SW_DB_HOST' not set.");
    }

    var conStrBuilder = new SqlConnectionStringBuilder(connectionString);
    conStrBuilder.Password = sqlPassword;
    conStrBuilder.DataSource = sqlServerHost;
    var connection = conStrBuilder.ConnectionString;
    
    builder.Services.AddDbContext<SolarWatchContext>(options =>
        options.UseSqlServer(connection));
}

/*void AddIdentity()
{
    builder.Services
        .AddIdentityCore<IdentityUser>(options =>
        {
            options.SignIn.RequireConfirmedAccount = false;
            options.User.RequireUniqueEmail = true;
            options.Password.RequireDigit = false;
            options.Password.RequiredLength = 4;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireLowercase = false;
        })
        //.AddRoles<IdentityRole>()
        .AddEntityFrameworkStores<SolarWatchContext>();
}*/



void AddEnvironmentVariables()
{
    var root = Directory.GetCurrentDirectory();
    var dotenv = Path.Combine(root, ".env");
    DotEnv.Load(dotenv);

    var config = new ConfigurationBuilder()
        .AddEnvironmentVariables()
        .Build();
}

public partial class Program { }