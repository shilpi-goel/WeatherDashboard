import { render, screen, fireEvent } from "@testing-library/react";
import SearchBar from "../../src/components/SearchBar";

describe("SearchBar", () => {
  test("renders input and button", () => {
    render(<SearchBar onSearch={jest.fn()} />);
    expect(screen.getByPlaceholderText("Enter city...")).toBeInTheDocument();
    expect(screen.getByText("Search")).toBeInTheDocument();
  });

  test("calls onSearch when input is not empty", () => {
    const onSearchMock = jest.fn();
    render(<SearchBar onSearch={onSearchMock} />);
    
    fireEvent.change(screen.getByPlaceholderText("Enter city..."), { target: { value: "Tokyo" } });
    fireEvent.click(screen.getByText("Search"));
    
    expect(onSearchMock).toHaveBeenCalledWith("Tokyo");
  });

  test("does not call onSearch for empty input", () => {
    const onSearchMock = jest.fn();
    render(<SearchBar onSearch={onSearchMock} />);
    fireEvent.click(screen.getByText("Search"));
    expect(onSearchMock).not.toHaveBeenCalled();
  });
});
