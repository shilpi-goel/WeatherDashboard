using WeatherApi.Models;

namespace WeatherApi.Services;

public interface IWeatherService
{
    Task<Result<WeatherDto>> GetWeatherAsync(string city);
}
