using WeatherApi.Services;

using Microsoft.Extensions.Caching.Memory;
using WeatherApi.Models;
using WeatherApi.Clients;

public class WeatherService : IWeatherService
{
    private readonly IOpenWeatherClient _client;
    private readonly IMemoryCache _cache;
    private readonly ILogger<WeatherService> _logger;

    public WeatherService(
        IOpenWeatherClient client,
        IMemoryCache cache,
        ILogger<WeatherService> logger)
    {
        _client = client;
        _cache = cache;
        _logger = logger;
    }

    public async Task<Result<WeatherDto>> GetWeatherAsync(string city)
    {
        if (string.IsNullOrWhiteSpace(city))
            return Result<WeatherDto>.Fail("City name is required");

        var cacheKey = $"weather_{city.ToLowerInvariant()}";

        if (_cache.TryGetValue(cacheKey, out WeatherDto cached))
        {
            _logger.LogInformation("Cache hit for {City}", city);
            return Result<WeatherDto>.Ok(cached);
        }

        try
        {
            var weather = await _client.FetchWeatherAsync(city);

            _cache.Set(cacheKey, weather, TimeSpan.FromMinutes(10));

            return Result<WeatherDto>.Ok(weather);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to fetch weather for {City}", city);
            return Result<WeatherDto>.Fail("Unable to retrieve weather data");
        }
    }
}
