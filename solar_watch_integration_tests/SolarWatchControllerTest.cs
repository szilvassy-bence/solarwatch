using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using FluentAssertions;
using Moq;
using solar_watch_backend.Models;
using solar_watch_backend.Models.Contracts;
using solar_watch_backend.Services.Authentication;
using solar_watch_backend.Services.LatLngProvider;
using solar_watch_backend.Services.SunriseSunsetProvider;
using solar_watch_integration_tests.helpers;
using Xunit.Abstractions;

namespace solar_watch_integration_tests;

public class SolarWatchControllerTest : IClassFixture<SolarWatchWebApplicationFactory>
{
    private readonly ITestOutputHelper _outputHelper;
    private readonly HttpClient _httpClient;
    

    public SolarWatchControllerTest(ITestOutputHelper outputHelper)
    {
        SolarWatchWebApplicationFactory _solarWatchFactory = new SolarWatchWebApplicationFactory();
        _outputHelper = outputHelper;
        _httpClient = _solarWatchFactory.CreateClient();
    }

    [Fact]
    public async Task GetAllCitiesTest()
    {
        // Arrange
        // Act
        var response = await _httpClient.GetAsync("/api/SolarWatch/GetAllCities");
        var result = await response.Content.ReadFromJsonAsync<List<City>>();

        // Assert
        response.EnsureSuccessStatusCode();
        response.Content.Headers.ContentType.ToString().Should().Be("application/json; charset=utf-8");
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        result.Count.Should().Be(3);
        result[2].Name.Should().Be("Budapest");
        result[0].Longitude.Should().Be(100);
        _outputHelper.WriteLine(result[0].ToString());
    }
    
    [Fact]
    public async Task GetCityByNameTestReturnsOKIfCityCorrect()
    {
        // Arrange
        // Act
        var cityResponse = await _httpClient.GetAsync($"/api/SolarWatch/cities/Budapest");

        // Assert
        cityResponse.EnsureSuccessStatusCode();
        Assert.Equal( HttpStatusCode.OK, cityResponse.StatusCode);
    }
    
    [Fact]
    public async Task GetCityByNameTestReturnsNotFoundIfCityIncorrect()
    {
        // Arrange
        // Act
        var cityResponse = await _httpClient.GetAsync($"/api/SolarWatch/cities/sdfsdfgsfg");

        // Assert
        Assert.Equal( HttpStatusCode.NotFound, cityResponse.StatusCode);
    }
    
    [Fact]
    public async Task DeleteCityByIdIfFoundAndReturnsNoContent()
    {
        // Arrange and act
        var deleteResponse = await _httpClient.DeleteAsync($"/api/SolarWatch/cities/1/delete");

        // Assert
        Assert.Equal( HttpStatusCode.NoContent, deleteResponse.StatusCode);
    }
    
    [Fact]
    public async Task GetSunInfoReturnsOkIfCityIsCorrect()
    {
        // Arrange and act
        var sunResponse1 = await _httpClient.GetAsync($"/api/SolarWatch/sunrisesunsets/Budapest/2024-05-02");
        var sunResponse2 = await _httpClient.GetAsync($"/api/SolarWatch/sunrisesunsets/London/2024-05-02");

        // Assert
        Assert.Equal( HttpStatusCode.OK, sunResponse1.StatusCode);
        Assert.Equal(HttpStatusCode.OK, sunResponse2.StatusCode);
    }
    
    [Fact]
    public async Task GetSunInfoReturnsNotFoundIfCityIsNotCorrect()
    {
        // Arrange and act
        var sunResponse1 = await _httpClient.GetAsync($"/api/SolarWatch/sunrisesunsets/Budaasdfgpest/2024-05-02");

        // Assert
        Assert.Equal( HttpStatusCode.NotFound, sunResponse1.StatusCode);
    }
    
    [Fact]
    public async Task DeleteSunInfoReturnsNoContentIfSuccessful()
    {
        // Arrange and act
        var sunResponse = await _httpClient.DeleteAsync($"/api/SolarWatch/sunrisesunsets/1/Delete");

        // Assert
        Assert.Equal( HttpStatusCode.NoContent, sunResponse.StatusCode);
    }
    
    [Fact]
    public async Task DeleteSunInfoReturnsNotFoundIfSunInfoNotFound()
    {
        // Arrange and act
        var sunResponse = await _httpClient.DeleteAsync($"/api/SolarWatch/sunrisesunsets/4/Delete");

        // Assert
        Assert.Equal( HttpStatusCode.NotFound, sunResponse.StatusCode);
    }

    [Fact]
    public async Task GetAllSunriseSunsetsReturnsOk()
    {
        // Arrange and act
        var response = await _httpClient.GetAsync("/api/SolarWatch/sunrisesunsets");
        
        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

}