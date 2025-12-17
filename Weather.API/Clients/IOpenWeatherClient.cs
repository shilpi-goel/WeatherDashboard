using WeatherApi.Models;

namespace WeatherApi.Clients;

public interface IOpenWeatherClient
{
    Task<WeatherDto> FetchWeatherAsync(string city);
}