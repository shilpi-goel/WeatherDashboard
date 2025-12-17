import React from "react";
import SearchBar from "./components/SearchBar";
import WeatherCard from "./components/WeatherCard";
import DefaultLocationForm from "./components/DefaultLocationForm";
import { useWeather } from "./hooks/useWeather";
import "./App.css";

export default function App() {
  const { weather, defaultCity, search, saveDefaultCity } = useWeather();

  return (
    <div className="app-container">
      <h1>Weather Dashboard</h1>

      <div className="controls">
         <DefaultLocationForm onSave={saveDefaultCity} defaultCity={defaultCity} />
        <SearchBar onSearch={search} />
      </div>

      {weather && (
        <div className="weather-display">
          <WeatherCard weather={weather} />
        </div>
      )}
    </div>
  );
}
