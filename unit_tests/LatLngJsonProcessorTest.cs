using solar_watch_backend.Models;
using solar_watch_backend.Services.LatLngProvider;

namespace unit_tests;

public class LatLngJsonProcessorTest
{
    private ILatLngJsonProcessor _latLngJsonProcessor;

    [SetUp]
    public void Setup()
    {
        _latLngJsonProcessor = new LatLngJsonProcessor();
    }


    [Test]
    public void ProcessCityJsonReturnsCorrectCity()
    {
        // Arrange
        string dataWithState = "[{\"name\":\"London\",\"lat\":51.5073219,\"lon\":-0.1276474,\"country\":\"GB\",\"state\":\"England\"}]";
        string dataWithoutState = "[{\"name\":\"London\",\"lat\":51.5073219,\"lon\":-0.1276474,\"country\":\"GB\"}]";
        
        // Act
        City cityWithState = _latLngJsonProcessor.Process(dataWithState);
        City cityWithoutState = _latLngJsonProcessor.Process(dataWithoutState);
        
        // Assert
        Assert.IsNotNull(cityWithState);
        Assert.That(cityWithoutState.Latitude, Is.EqualTo(51.5073219));
        Assert.IsNull(cityWithoutState.State);
        Assert.IsNotNull(cityWithState.State);
        Assert.That(cityWithState.State, Is.EqualTo("England"));
    }

    [Test]
    public void EmptyStringThrowsException()
    {
        // Arrange
        string data = "";
        
        // Act
        // Assert
        Assert.Throws<ArgumentException>(() => _latLngJsonProcessor.Process(data));
    }
    
    [Test]
    public void IncompleteStringThrowsException()
    {
        // Arrange
        string data = "[{\"name\":\"London\",\"lon\":-0.1276474,\"country\":\"GB\",\"state\":\"England\"}]";
        
        // Act
        // Assert
        Assert.Throws<KeyNotFoundException>(() => _latLngJsonProcessor.Process(data));
    }
}