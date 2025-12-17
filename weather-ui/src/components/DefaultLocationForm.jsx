import React, { useState, useEffect } from "react";

export default function DefaultLocationForm({ defaultCity, onSave }) {
  const [city, setCity] = useState(defaultCity || "");

  useEffect(() => {
    setCity(defaultCity || "");
  }, [defaultCity]);
  
  const handleSave = () => {
    if (city.trim()) onSave(city.trim());
  };

  return (
    <div>
      <input
        type="text"
        value={city}
        onChange={(e) => setCity(e.target.value)}
        placeholder="Set default city"
      />
      <button onClick={handleSave}>Save</button>
    </div>
  );
}
