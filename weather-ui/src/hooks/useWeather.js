import { useState, useEffect } from "react";
import { fetchWeather, fetchDefault, saveDefault  } from "../services/weatherApi";

export function useWeather() {
  const [weather, setWeather] = useState(null);
   const [defaultCity, setDefaultCity] = useState("");

  const search = async (city) => {
    try {
      const data = await fetchWeather(city);      
      setWeather(data);
    } catch (e) {
            console.error(e);

      alert("Invalid city");
    }
  };

  const saveDefaultCity = async (city) => {
    try {
      await saveDefault(city);
      setDefaultCity(city);
      await search(city); // fetch weather immediately after saving
    } catch (err) {
      console.error(err.message);
    }
  };

  const loadDefault = async () => {
    try {
      const city = await fetchDefault();
      if (city) {
        setDefaultCity(city.city);
        await search(city.city);
      }
    } catch (err) {
      console.error(err.message);
    }
  };

  useEffect(() => {
    loadDefault();
  }, []);

  return { weather, defaultCity, search, saveDefaultCity };
}
