namespace WeatherApi.Services;

public interface IDefaultLocationService
{
    Task SetDefaultCityAsync(string city);

    Task<string> GetDefaultCityAsync();
}