import { useState } from "react";

export default function SearchBar({ onSearch }) {
  const [city, setCity] = useState("");

  const submit = () => {
    if (city.trim() !== "") {
      onSearch(city);
    }
  };

  return (
    <div>
      <input
        placeholder="Enter city..."
        value={city}
        onChange={e => setCity(e.target.value)}
      />
      <button onClick={submit}>Search</button>
    </div>
  );
}
