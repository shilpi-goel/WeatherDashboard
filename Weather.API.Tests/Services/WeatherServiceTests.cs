using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;
using WeatherApi.Clients;
using WeatherApi.Models;
using WeatherApi.Services;
using Xunit;

namespace WeatherApi.Tests.Services
{
    public class WeatherServiceTests
    {
        private readonly Mock<IOpenWeatherClient> _clientMock;
        private readonly Mock<IMemoryCache> _cacheMock;
        private readonly Mock<ILogger<WeatherService>> _loggerMock;
        private readonly WeatherService _service;

        public WeatherServiceTests()
        {
            _clientMock = new Mock<IOpenWeatherClient>();
            _cacheMock = new Mock<IMemoryCache>();
            _loggerMock = new Mock<ILogger<WeatherService>>();
            _service = new WeatherService(_clientMock.Object, _cacheMock.Object, _loggerMock.Object);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("   ")]
        public async Task GetWeatherAsync_ReturnsFail_WhenCityIsNullOrWhitespace(string city)
        {
            // Act
            var result = await _service.GetWeatherAsync(city);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("City name is required", result.Error);
        }

        [Fact]
        public async Task GetWeatherAsync_ReturnsCachedResult_WhenCacheHit()
        {
            // Arrange
            var city = "Berlin";
            var cacheKey = $"weather_{city.ToLowerInvariant()}";
            var cachedWeather = new WeatherDto { City = city, Temperature = 10, Humidity = 50, WindSpeed = 2, Icon = "01d" };

            object outObj = cachedWeather;
            _cacheMock.Setup(c => c.TryGetValue(cacheKey, out outObj)).Returns(true);

            // Act
            var result = await _service.GetWeatherAsync(city);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(cachedWeather, result.Value);
            _clientMock.Verify(c => c.FetchWeatherAsync(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task GetWeatherAsync_FetchesAndCaches_WhenCacheMiss()
        {
            // Arrange
            var city = "Paris";
            var cacheKey = $"weather_{city.ToLowerInvariant()}";
            var weather = new WeatherDto { City = city, Temperature = 15, Humidity = 60, WindSpeed = 3, Icon = "02d" };

            object outObj = null;
            _cacheMock.Setup(c => c.TryGetValue(cacheKey, out outObj)).Returns(false);
            _clientMock.Setup(c => c.FetchWeatherAsync(city)).ReturnsAsync(weather);
            _cacheMock
                .Setup(c => c.Set(
                    cacheKey,
                    It.IsAny<WeatherDto>(),
                    It.IsAny<TimeSpan>()))
                .Returns((object key, object value, TimeSpan t) => value);

            // Act
            var result = await _service.GetWeatherAsync(city);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(weather, result.Value);
            _clientMock.Verify(c => c.FetchWeatherAsync(city), Times.Once);
            _cacheMock.Verify(c => c.Set(cacheKey, weather, It.IsAny<TimeSpan>()), Times.Once);
        }

        [Fact]
        public async Task GetWeatherAsync_ReturnsFail_WhenFetchThrows()
        {
            // Arrange
            var city = "Rome";
            var cacheKey = $"weather_{city.ToLowerInvariant()}";
            object outObj = null;
            _cacheMock.Setup(c => c.TryGetValue(cacheKey, out outObj)).Returns(false);
            _clientMock.Setup(c => c.FetchWeatherAsync(city)).ThrowsAsync(new Exception("API error"));

            // Act
            var result = await _service.GetWeatherAsync(city);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Unable to retrieve weather data", result.Error);
            _cacheMock.Verify(c => c.Set(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<TimeSpan>()), Times.Never);
        }
    }
}
