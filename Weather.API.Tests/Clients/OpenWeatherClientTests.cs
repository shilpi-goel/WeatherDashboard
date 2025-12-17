namespace WeatherApi.Tests.Clients;

using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using WeatherApi.Clients;
using WeatherApi.Models;
using Xunit;

public class OpenWeatherClientTests
{
    private static OpenWeatherClient CreateClient(
        HttpResponseMessage httpResponse,
        string apiKey = "test-api-key")
    {
        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(httpResponse);

        var httpClient = new HttpClient(handlerMock.Object);

        var configMock = new Mock<IConfiguration>();
        configMock.Setup(c => c["OpenWeather:ApiKey"]).Returns(apiKey);

        var loggerMock = new Mock<ILogger<OpenWeatherClient>>();

        return new OpenWeatherClient(httpClient, configMock.Object, loggerMock.Object);
    }

    [Fact]
    public async Task FetchWeatherAsync_ReturnsMockData_WhenApiKeyMissing()
    {
        // Arrange
        var client = CreateClient(new HttpResponseMessage(), apiKey: null);

        // Act
        var result = await client.FetchWeatherAsync("London");

        // Assert
        Assert.Equal("London", result.City);
        Assert.Equal(20, result.Temperature);
        Assert.Equal(60, result.Humidity);
        Assert.Equal(5, result.WindSpeed);
        Assert.Equal("01d", result.Icon);
    }

    [Fact]
    public async Task FetchWeatherAsync_ReturnsWeatherDto_WhenApiCallSuccessful()
    {
        // Arrange
        var openWeatherResponse = new OpenWeatherResponse
        {
            main = new OpenWeatherResponse.Main { temp = 15, humidity = 80 },
            wind = new OpenWeatherResponse.Wind { speed = 3.5 },
            weather = new[] { new OpenWeatherResponse.Weather { icon = "10d" } }
        };

        var httpResponse = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(openWeatherResponse)
        };

        var client = CreateClient(httpResponse);

        // Act
        var result = await client.FetchWeatherAsync("Paris");

        // Assert
        Assert.Equal("Paris", result.City);
        Assert.Equal(15, result.Temperature);
        Assert.Equal(80, result.Humidity);
        Assert.Equal(3.5, result.WindSpeed);
        Assert.Equal("10d", result.Icon);
    }

    [Fact]
    public async Task FetchWeatherAsync_Throws_WhenApiReturnsInvalidData()
    {
        // Arrange: missing main/wind/weather
        var openWeatherResponse = new OpenWeatherResponse();
        var httpResponse = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(openWeatherResponse)
        };
        var client = CreateClient(httpResponse);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => client.FetchWeatherAsync("Nowhere"));
    }

    [Fact]
    public async Task FetchWeatherAsync_Throws_WhenHttpFails()
    {
        // Arrange
        var httpResponse = new HttpResponseMessage(HttpStatusCode.InternalServerError);
        var client = CreateClient(httpResponse);

        // Act & Assert
        await Assert.ThrowsAsync<HttpRequestException>(() => client.FetchWeatherAsync("ErrorCity"));
    }
}
