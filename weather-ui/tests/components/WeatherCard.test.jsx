import { render, screen } from "@testing-library/react";
import WeatherCard from "../../src/components/WeatherCard";

describe("WeatherCard", () => {
  const weather = { city: "London", temperature: 20, humidity: 50, windSpeed: 10 };

  test("renders weather info", () => {
    render(<WeatherCard weather={weather} />);
    expect(screen.getByText("London")).toBeInTheDocument();
    expect(screen.getByText("Temperature: 20 °C")).toBeInTheDocument();
    expect(screen.getByText("Humidity: 50 %")).toBeInTheDocument();
    expect(screen.getByText("Wind: 10 km/h")).toBeInTheDocument();
  });

  test("renders nothing when weather is null", () => {
    const { container } = render(<WeatherCard weather={null} />);
    expect(container).toBeEmptyDOMElement();
  });
});
