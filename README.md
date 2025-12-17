# Weather Dashboard

A full-stack Weather Dashboard application built with:

- **Frontend:** React + Vite
- **Backend:** ASP.NET Core Web API
- **Weather Data:** OpenWeather API (or similar external provider)
- **Testing:** Jest + React Testing Library

The dashboard allows users to:

- Search for weather by city
- Save and display a default location
- View conditions such as temperature, humidity, and wind speed
- Display reusable weather cards
- Communicate with a backend API for secure data retrieval
- Avoid exposing API keys on the frontend

---

## 📁 Project Structure

WeatherDashboard/
│
├── weather-ui/ # Frontend (React)
│ ├── src/
│ ├── tests/
│ ├── public/
│ └── vite.config.js
│
└── weather-api/ # Backend (.NET Core API)
├── Controllers/
├── Services/
├── Models/
├── appsettings.json
└── Program.cs


---

## 🚀 Frontend (weather-ui)

### 🛠️ Install Dependencies

```bash
cd weather-ui
npm install

### 🖥️ Windows PowerShell Users Only
Set-ExecutionPolicy -Scope Process -ExecutionPolicy Bypass

### Run Dev Server
npm run dev

### Build for Production
npm run build
```bash

### Run Tests
```bash
npm test

```
### Flow in Weather App

1️⃣ User types city
2️⃣ OnSearch event triggers
3️⃣ useWeather() fetches API
4️⃣ state updates
5️⃣ components re-render
6️⃣ UI shows new data

---
## 🚀 Backend (weather-api)
### 🛠️ Install Dependencies
```bash
cd weather-api
dotnet restore
```
### Configure API Key
- Store your weather API key securely in `appsettings.json` or use environment variables.
- {
  "WeatherApi": {
    "ApiKey": "<YOUR_API_KEY>",
    "BaseUrl": "https://api.openweathermap.org/data/2.5/"
  }

### Run the API
```bash
dotnet run
```
### Build for Production
```bash
dotnet publish -c Release
```
### Run Tests
```bash
dotnet test
```
---
## 🔑 Environment Variables
Both frontend and backend require environment variables for configuration, especially for API keys.
- **Frontend:** Create a `.env` file in `weather-ui/` with your OpenWeather API key.
- **Backend:** Store sensitive keys in `appsettings.json` or use environment variables.
- Ensure API keys are not exposed in the frontend code.
- Use the backend to securely fetch weather data.
- Refer to the documentation of the weather API provider for specific environment variable names.

---
## 📄 License
This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.


