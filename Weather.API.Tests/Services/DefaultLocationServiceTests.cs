namespace WeatherApi.Tests.Services;

using System.Threading.Tasks;
using WeatherApi.Services;
using Xunit;

public class DefaultLocationServiceTests
{
    [Fact]
    public async Task GetDefaultCityAsync_ReturnsDefaultLondonInitially()
    {
        // Arrange
        var service = new DefaultLocationService();

        // Act
        var city = await service.GetDefaultCityAsync();

        // Assert
        Assert.Equal("London", city);
    }

    [Fact]
    public async Task SetDefaultCityAsync_ChangesDefaultCity()
    {
        // Arrange
        var service = new DefaultLocationService();

        // Act
        await service.SetDefaultCityAsync("Berlin");
        var city = await service.GetDefaultCityAsync();

        // Assert
        Assert.Equal("Berlin", city);
    }

    [Fact]
    public async Task SetDefaultCityAsync_CanSetMultipleTimes()
    {
        // Arrange
        var service = new DefaultLocationService();

        // Act & Assert
        await service.SetDefaultCityAsync("Paris");
        Assert.Equal("Paris", await service.GetDefaultCityAsync());

        await service.SetDefaultCityAsync("Tokyo");
        Assert.Equal("Tokyo", await service.GetDefaultCityAsync());
    }
}
