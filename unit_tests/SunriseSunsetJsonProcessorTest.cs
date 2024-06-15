using solar_watch_backend.Models;
using solar_watch_backend.Services.SunriseSunsetProvider;

namespace unit_tests;

public class SunriseSunsetJsonProcessorTest
{
    private ISunriseSunsetJsonProcessor _sunriseSunsetJsonProcessor;

    [SetUp]
    public void Setup()
    {
        _sunriseSunsetJsonProcessor = new SunriseSunsetJsonProcessor();
    }
    
    [Test]
    public void ProcessCityJsonReturnsCorrectCity()
    {
        // Arrange
        string sunriseSunsetString = "{\"results\":{\"sunrise\":\"7:27:02 AM\",\"sunset\":\"5:05:55 PM\"}}";
        
        // Act
        SunriseSunset sunriseSunset = _sunriseSunsetJsonProcessor.Process(sunriseSunsetString, new DateTime());
        
        // Assert
        Assert.IsNotNull(sunriseSunset);
        Assert.IsInstanceOf<DateTime>(sunriseSunset.Sunrise);
    }
    
    [Test]
    public void EmptyStringThrowsException()
    {
        // Arrange
        string data = "";
        
        // Act
        // Assert
        Assert.Throws<ArgumentException>(() => _sunriseSunsetJsonProcessor.Process(data, new DateTime()));
    }
    
    [Test]
    public void IncompleteStringThrowsException()
    {
        // Arrange
        string data = "{\"results\":{\"sunrise\":\"7:27:02 AM\"}}";
        
        // Act
        // Assert
        Assert.Throws<KeyNotFoundException>(() => _sunriseSunsetJsonProcessor.Process(data, new DateTime()));
    }
}