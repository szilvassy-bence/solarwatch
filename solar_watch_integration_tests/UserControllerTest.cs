using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using solar_watch_backend.Models;
using solar_watch_backend.Models.Contracts;
using solar_watch_backend.Services.SunriseSunsetProvider;
using solar_watch_integration_tests.helpers;
using Xunit.Abstractions;

namespace solar_watch_integration_tests;

public class UserControllerTest : IClassFixture<SolarWatchWebApplicationFactory>
{
    private readonly HttpClient _httpClient;
    private readonly ITestOutputHelper _outputHelper;
    private readonly SolarWatchWebApplicationFactory _factory;
    private readonly IServiceProvider _serviceProvider;

    public UserControllerTest(ITestOutputHelper outputHelper)
    {
        _outputHelper = outputHelper;
        _factory = new SolarWatchWebApplicationFactory();
        _httpClient = _factory.CreateClient();
        _serviceProvider = _factory.Services;
    }

    [Fact]
    public async Task GetAllReturnsOkIfAdmin()
    {
        // Arrange 
        var token = await Helper.LoginAdmin(_httpClient);
        
        // Act
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        var response = await _httpClient.GetAsync("/api/user/get-all");
        var result = await response.Content.ReadFromJsonAsync<List<ApplicationUser>>();
        
        // Assert
        response.EnsureSuccessStatusCode();
        response.Content.Headers.ContentType.ToString().Should().Be("application/json; charset=utf-8");
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        result.Count.Should().Be(1);
    }
    
    [Fact]
    public async Task GetAllReturnsUnauthorizedIfNotLoggedIn()
    {
        // Arrange 
        // Act
        var response = await _httpClient.GetAsync("/api/user/get-all");
        
        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task GetAllReturnsForbiddenIfUser()
    {
        // Arrange 
        var token = await Helper.LoginUser(_httpClient);
        
        // Act
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        var response = await _httpClient.GetAsync("/api/user/get-all");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task GetReturnsOkIfLoggedIn()
    {
        // Arrange
        var token = await Helper.LoginUser(_httpClient);
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        
        // Act
        var response = await _httpClient.GetAsync("/api/user/get");
        var responseContent = await response.Content.ReadAsStringAsync();
        _outputHelper.WriteLine(responseContent); // Log the response content

        ApplicationUser result = null;
        try
        {
            var options = new JsonSerializerOptions{PropertyNamingPolicy = JsonNamingPolicy.CamelCase};
            result = JsonSerializer.Deserialize<ApplicationUser>(responseContent, options);
        }
        catch (JsonException ex)
        {
            _outputHelper.WriteLine($"JSON deserialization error: {ex.Message}");
        }
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        result.IdentityUser.UserName.Should().Be("user");
    }

    [Fact]
    public async Task UpdateReturnsOkIfSuccessful()
    {
        // Arrange
        var token = await Helper.LoginUser(_httpClient);

        var userDataChange = new UserDataChange("new@new", "new");
        var jsonContent = new StringContent(JsonSerializer.Serialize(userDataChange), Encoding.UTF8, "application/json");
        
        var request = new HttpRequestMessage(HttpMethod.Patch, "/api/user/update")
        {
            Content = jsonContent
        };
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        
        // Act
        var response = await _httpClient.SendAsync(request);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Fact]
    public async Task UpdateReturnsBadRequestIfUserDataChangeIsNotCorrect()
    {
        // Arrange
        var token = await Helper.LoginUser(_httpClient);

        var userDataChange = new UserDataChange("newnew", "new");
        var jsonContent = new StringContent(JsonSerializer.Serialize(userDataChange), Encoding.UTF8, "application/json");
        
        var request = new HttpRequestMessage(HttpMethod.Patch, "/api/user/update")
        {
            Content = jsonContent
        };
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        
        // Act
        var response = await _httpClient.SendAsync(request);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    /*[Fact]
    public async Task DeleteReturnsNoContentIfSuccessful()
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var seeder = scope.ServiceProvider.GetRequiredService<Seeder>();
            await seeder.InitializeIdentityUserDbForTests();
        }
        
        // Arrange
        var token = await Helper.LoginUser(_httpClient);
        
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        
        // Act
        var response = await _httpClient.DeleteAsync("/api/user/delete-user");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }*/

    [Fact]
    public async Task UpdatePasswordReturnsOKIfSuccessful()
    {
        // Arrange
        var token = await Helper.LoginUser(_httpClient);

        var userPasswordChange = new UserPasswordChange("password", "password2");
        var jsonContent = new StringContent(JsonSerializer.Serialize(userPasswordChange), Encoding.UTF8,
            "application/json");

        var request = new HttpRequestMessage(HttpMethod.Patch, "/api/user/update-password")
        {
            Content = jsonContent
        };
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // Act
        var response = await _httpClient.SendAsync(request);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Fact]
    public async Task UpdatePasswordReturnsNotFoundIfNotSuccessful()
    {
        // Arrange
        var token = await Helper.LoginUser(_httpClient);

        var userPasswordChange = new UserPasswordChange("password", "pas");
        var jsonContent = new StringContent(JsonSerializer.Serialize(userPasswordChange), Encoding.UTF8,
            "application/json");

        var request = new HttpRequestMessage(HttpMethod.Patch, "/api/user/update-password")
        {
            Content = jsonContent
        };
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // Act
        var response = await _httpClient.SendAsync(request);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task FavoritesReturnsOk()
    {
        // Arrange
        var token = await Helper.LoginUser(_httpClient);
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        
        // Act
        var response = await _httpClient.GetAsync("/api/user/favorites");
        var result = await response.Content.ReadFromJsonAsync<ICollection<City>>();
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Count.Should().Be(1);
    }
    
    [Fact]
    public async Task AddFavoritesReturnsOk()
    {
        // Arrange
        var token = await Helper.LoginUser(_httpClient);
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        
        // Act
        var request = new HttpRequestMessage(HttpMethod.Post, "/api/user/2/add-city")
        {};
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        
        // Act
        var response = await _httpClient.SendAsync(request);
        var result = await response.Content.ReadFromJsonAsync<City>();
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Name.Should().Be("Debrecen");
    }
    
    [Fact]
    public async Task DeleteFavoritesReturnsOk()
    {
        // Arrange
        var token = await Helper.LoginUser(_httpClient);
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        
        // Act
        var request = new HttpRequestMessage(HttpMethod.Delete, "/api/user/1/delete-city");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        
        // Act
        var response = await _httpClient.SendAsync(request);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
    
    [Fact]
    public async Task DeleteFavoritesReturnsBadRequestIfNotInFavorites()
    {
        // Arrange
        var token = await Helper.LoginUser(_httpClient);
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        
        // Act
        var request = new HttpRequestMessage(HttpMethod.Delete, "/api/user/2/delete-city");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        
        // Act
        var response = await _httpClient.SendAsync(request);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}