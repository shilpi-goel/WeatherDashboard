namespace Weather.API.Tests.Controllers;

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WeatherApi.Controllers;
using WeatherApi.Models;
using WeatherApi.Services;
using Xunit;

public class WeatherControllerTests
{
    private readonly Mock<IWeatherService> _weatherServiceMock;
    private readonly WeatherController _controller;

    public WeatherControllerTests()
    {
        _weatherServiceMock = new Mock<IWeatherService>();
        _controller = new WeatherController(_weatherServiceMock.Object);
    }

    [Fact]
    public async Task Get_ReturnsOk_WhenServiceReturnsSuccess()
    {
        // Arrange
        var city = "London";
        var weather = new WeatherDto { City = city, Temperature = 20, Humidity = 50, WindSpeed = 5, Icon = "sunny" };
        _weatherServiceMock
            .Setup(s => s.GetWeatherAsync(city))
            .ReturnsAsync(Result<WeatherDto>.Ok(weather));

        // Act
        var result = await _controller.Get(city);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(weather, okResult.Value);
    }

    [Fact]
    public async Task Get_ReturnsBadRequest_WhenServiceReturnsFailure()
    {
        // Arrange
        var city = "Unknown";
        var error = "City not found";
        _weatherServiceMock
            .Setup(s => s.GetWeatherAsync(city))
            .ReturnsAsync(Result<WeatherDto>.Fail(error));

        // Act
        var result = await _controller.Get(city);

        // Assert
        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(error, badRequest.Value);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("   ")]
    public async Task Get_ReturnsBadRequest_WhenCityIsNullOrWhitespace(string city)
    {
        // Act
        var result = await _controller.Get(city);

        // Assert
        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("City name is required.", badRequest.Value);
    }
}
