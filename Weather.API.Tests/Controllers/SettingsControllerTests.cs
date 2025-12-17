namespace Weather.API.Tests.Controllers;


using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json.Linq;
using WeatherApi.Controllers;
using WeatherApi.Models;
using WeatherApi.Services;
using Xunit;

public class SettingsControllerTests
{
    private readonly Mock<IDefaultLocationService> _defaultLocationServiceMock;
    private readonly SettingsController _controller;

    public SettingsControllerTests()
    {
        _defaultLocationServiceMock = new Mock<IDefaultLocationService>();
        _controller = new SettingsController(_defaultLocationServiceMock.Object);
    }

    [Fact]
    public async Task SetDefaultLocation_ReturnsOk_WhenCityIsValid()
    {
        // Arrange
        var request = new DefaultLocationRequest { City = "Paris" };

        // Act
        var result = await _controller.SetDefaultLocation(request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var messageValue = okResult.Value?.GetType().GetProperty("Message")?.GetValue(okResult.Value, null);
        Assert.Equal("Default location updated.", messageValue);        
        _defaultLocationServiceMock.Verify(s => s.SetDefaultCityAsync("Paris"), Times.Once);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("   ")]
    public async Task SetDefaultLocation_ReturnsBadRequest_WhenCityIsNullOrWhitespace(string city)
    {
        // Arrange
        var request = new DefaultLocationRequest { City = city };

        // Act
        var result = await _controller.SetDefaultLocation(request);

        // Assert
        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("City is required.", badRequest.Value);
        _defaultLocationServiceMock.Verify(s => s.SetDefaultCityAsync(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task GetDefaultLocation_ReturnsOk_WithCity()
    {
        // Arrange
        var city = "Berlin";
        _defaultLocationServiceMock.Setup(s => s.GetDefaultCityAsync()).ReturnsAsync(city);

        // Act
        var result = await _controller.GetDefaultLocation();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var cityValue = okResult.Value?.GetType().GetProperty("City")?.GetValue(okResult.Value, null);
        Assert.Equal(city, cityValue);
        _defaultLocationServiceMock.Verify(s => s.GetDefaultCityAsync(), Times.Once);
    }
}
