export async function fetchWeather(city) {
  const response = await fetch('/api/weather/' + encodeURIComponent(city));
  if (!response.ok) throw new Error("No data");
  return await response.json();
}

export async function saveDefault(city) {
  await fetch('/api/settings/default-location', {
    method: 'POST',
    body: JSON.stringify({ city }),
    headers: { 'Content-Type': 'application/json' }
  });
}

export async function fetchDefault() {
  const response = await fetch('/api/settings/default-location');
  return await response.json();
}
