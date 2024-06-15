using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using solar_watch_backend.Controllers;
using solar_watch_backend.Models;
using solar_watch_backend.Services.Repositories;

namespace unit_tests;

public class SolarWatchControllerTest
{
    private Mock<ILogger<SolarWatchController>> _loggerMock;
    private Mock<ISolarWatchRepository> _repositoryMock;
    private SolarWatchController _solarWatchController;
    
    [SetUp]
    public void SetUp()
    {
        _loggerMock = new Mock<ILogger<SolarWatchController>>();
        _repositoryMock = new Mock<ISolarWatchRepository>();
        _solarWatchController = new SolarWatchController(_loggerMock.Object, _repositoryMock.Object);
    }

    [Test]
    public async Task GetCityReturnsNotFoundIfRepositoryFails()
    {
        // Arrange
        var city = "{}";
        _repositoryMock.Setup(x => x.GetCityByName(It.IsAny<string>())).ThrowsAsync(new Exception());
        
        // Act
        var result = await _solarWatchController.GetCity("lkndfklkjldfg");
        
        // Assert
        Assert.IsInstanceOf(typeof(NotFoundObjectResult), result.Result);
    }

    [Test]
    public async Task GetCityReturnsOkResultIfCityIsValid()
    {
        // Arrange
        var expectedCity = new City();
        _repositoryMock.Setup(x => x.GetCityByName(It.IsAny<string>())).ReturnsAsync(expectedCity);
        
        // Act
        var result = await _solarWatchController.GetCity("Anything");
        
        // Assert
        Assert.IsInstanceOf(typeof(OkObjectResult), result.Result);
        Assert.That(((OkObjectResult)result.Result).Value, Is.EqualTo(expectedCity));
    }

    [Test]
    public async Task GetSunInfoByCityByDateReturnsValidDataIfSuccessful()
    {
        // Arrange
        var sunInfo = new SunriseSunset();
        _repositoryMock.Setup(x => x.GetSunriseSunsetByCityByDate(
            It.IsAny<string>(), It.IsAny<DateTime>()))
            .ReturnsAsync(sunInfo);
        
        // Act
        var result = await _solarWatchController.GetSunInfoByCityByDate("kjabsdfg", new DateTime(2024 - 01 - 01));
        
        Assert.IsInstanceOf(typeof(OkObjectResult), result.Result);
        Assert.That(((OkObjectResult)result.Result).Value, Is.EqualTo(sunInfo));
    }

    [Test]
    public async Task GetSunInfoByCityByDateThrowsExceptionReturnsNotFound()
    {
        // Arrange
        _repositoryMock.Setup(x => x.GetSunriseSunsetByCityByDate(It.IsAny<string>(), It.IsAny<DateTime>()))
            .ThrowsAsync(new Exception());
        
        // Act
        var result = await _solarWatchController.GetSunInfoByCityByDate("miasdfg", new DateTime(2024 - 01 - 01));
        
        // Assert
        Assert.IsInstanceOf(typeof(NotFoundObjectResult), result.Result);
    }
}