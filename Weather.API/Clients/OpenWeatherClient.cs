namespace WeatherApi.Clients;

using System.Net.Http.Json;
using WeatherApi.Models;

public class OpenWeatherClient : IOpenWeatherClient
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private readonly ILogger<OpenWeatherClient> _logger;

    public OpenWeatherClient(HttpClient httpClient, IConfiguration configuration, ILogger<OpenWeatherClient> logger)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<WeatherDto> FetchWeatherAsync(string city) // Updated return type to match the interface
    {
        var apiKey = _configuration["OpenWeather:ApiKey"];

        if (string.IsNullOrEmpty(apiKey))
        {
            _logger.LogWarning("OpenWeather API key is missing. Returning mock data for city: {City}", city);
            return new WeatherDto
            {
                City = city,
                Temperature = 20,
                Humidity = 60,
                WindSpeed = 5,
                Icon = "01d"
            };
        }

        string url = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={apiKey}&units=metric";

        try
        {
            _logger.LogInformation("Fetching weather data for city: {City}", city);

            var response = await _httpClient.GetAsync(url);

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadFromJsonAsync<OpenWeatherResponse>();

            if (json == null || json.main == null || json.wind == null || json.weather == null || json.weather.Length == 0)
            {
                _logger.LogError("Invalid response received from OpenWeather API for city: {City}", city);
                throw new InvalidOperationException("Invalid response received from OpenWeather API.");
            }

            return new WeatherDto
            {
                City = city,
                Temperature = json.main.temp,
                Humidity = json.main.humidity,
                WindSpeed = json.wind.speed,
                Icon = json.weather[0].icon
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching weather data for city: {City}", city);
            throw;
        }
    }
}
