export default function WeatherCard({ weather }) {
  if (!weather) return null;

  return (
    <div>
      <h2>{weather.city}</h2>
      <p>Temperature: {weather.temperature} °C</p>
      <p>Humidity: {weather.humidity} %</p>
      <p>Wind: {weather.windSpeed} km/h</p>
    </div>
  );
}
