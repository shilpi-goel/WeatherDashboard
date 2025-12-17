using Microsoft.AspNetCore.Mvc;
using WeatherApi.Models;
using WeatherApi.Services;

namespace WeatherApi.Controllers;

[ApiController]
[Route("api/settings")]
public class SettingsController : ControllerBase
{
    private readonly IDefaultLocationService _defaultLocationService;

    public SettingsController(IDefaultLocationService defaultLocationService)
    {
        _defaultLocationService = defaultLocationService;
    }

    [HttpPost("default-location")]
    public async Task<IActionResult> SetDefaultLocation([FromBody] DefaultLocationRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.City))
            return BadRequest("City is required.");

        await _defaultLocationService.SetDefaultCityAsync(request.City);

        return Ok(new { Message = "Default location updated." });
    }

    [HttpGet("default-location")]
    public async Task<IActionResult> GetDefaultLocation()
    {
        var city = await _defaultLocationService.GetDefaultCityAsync();
        return Ok(new { City = city });
    }
}
