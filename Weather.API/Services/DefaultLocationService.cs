namespace WeatherApi.Services;

public class DefaultLocationService: IDefaultLocationService
{
    private string _defaultCity = "London"; // fallback

    public Task SetDefaultCityAsync(string city)
    {
        _defaultCity = city;
        return Task.CompletedTask;
    }

    public Task<string> GetDefaultCityAsync() =>
        Task.FromResult(_defaultCity);
}
