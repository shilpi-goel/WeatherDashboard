namespace WeatherApi.Models;

public class WeatherDto
{
    public string City { get; set; }
    public double Temperature { get; set; }
    public int Humidity { get; set; }
    public double WindSpeed { get; set; }
    public string Icon { get; set; }
}

