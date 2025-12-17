namespace WeatherApi.Models;

public class OpenWeatherResponse
{
    public Main main { get; set; }
    public Wind wind { get; set; }
    public Weather[] weather { get; set; }

    public class Main
    {
        public double temp { get; set; }
        public int humidity { get; set; }
    }

    public class Wind
    {
        public double speed { get; set; }
    }

    public class Weather
    {
        public string icon { get; set; }
    }
}
