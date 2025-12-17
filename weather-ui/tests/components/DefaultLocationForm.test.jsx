import { render, screen, fireEvent } from "@testing-library/react";
import DefaultLocationForm from "../../src/components/DefaultLocationForm";

describe("DefaultLocationForm", () => {
  test("input displays default city", () => {
    render(<DefaultLocationForm defaultCity="London" onSave={jest.fn()} />);
    expect(screen.getByDisplayValue("London")).toBeInTheDocument();
  });

  test("calls onSave with trimmed value", () => {
    const onSaveMock = jest.fn();
    render(<DefaultLocationForm defaultCity="" onSave={onSaveMock} />);
    
    fireEvent.change(screen.getByPlaceholderText("Set default city"), { target: { value: "  Paris  " } });
    fireEvent.click(screen.getByText("Save"));
    
    expect(onSaveMock).toHaveBeenCalledWith("Paris");
  });
});
