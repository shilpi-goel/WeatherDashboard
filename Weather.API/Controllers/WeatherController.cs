namespace WeatherApi.Controllers;

using Microsoft.AspNetCore.Mvc;
using WeatherApi.Services;

[ApiController]
[Route("api/weather")]
public class WeatherController : ControllerBase
{
    private readonly IWeatherService _weatherService;

    public WeatherController(IWeatherService weatherService)
    {
        _weatherService = weatherService;
    }

    [HttpGet("{city}")]
    public async Task<IActionResult> Get(string city)
    {
        if (string.IsNullOrWhiteSpace(city))
            return BadRequest("City name is required.");

        var result = await _weatherService.GetWeatherAsync(city);

        return result.IsSuccess
            ? Ok(result.Value)
            : BadRequest(result.Error);
    }
}