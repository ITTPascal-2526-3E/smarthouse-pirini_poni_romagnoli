using BlaisePascal.SmartHouse.Domain.Abstraction;
using BlaisePascal.SmartHouse.Domain.ValueObjects;
using System;

namespace BlaisePascal.SmartHouse.Domain.Food
{
    public class Fridge : Device
    {
        // Manufacturer brand
        public string Brand { get; private set; }
        // Fridge model name or number
        public string Model { get; private set; }
        // Capacity in liters
        public int CapacityLiters { get; private set; }

        // Indicates if the fridge light is on
        public bool IsLightOn { get; private set; } // Renamed for consistency

        // Fridge temperatures
        // Current temperature
        public Temperature CurrentTemperature { get; private set; }
        public const double MIN_FRIDGE_TEMPERATURE_CELSIUS = 0.0;
        public const double MAX_FRIDGE_TEMPERATURE_CELSIUS = 6.0;
        public const double STANDARD_FRIDGE_TEMPERATURE_CELSIUS = 4.0;

        // Indicates if the fridge door is open
        public bool IsDoorOpen { get; private set; }


        public Fridge(string brand, string model, int capacityLiters, string name)
            : base(name, true) // Fridge usually starts ON
        {
            Brand = brand;
            Model = model;
            CapacityLiters = capacityLiters;

            CurrentTemperature = new Temperature(STANDARD_FRIDGE_TEMPERATURE_CELSIUS); // Default fridge temperature

            IsDoorOpen = false;
            IsLightOn = false;
            Touch();
        }

        // Opens the fridge door
        public override void ToggleOn()
        {
            IsDoorOpen = true;
            IsLightOn = true;
            Touch();
        }

        // Closes the fridge door
        public override void ToggleOff()
        {
            IsDoorOpen = false;
            IsLightOn = false;
            Touch();
        }

        // Sets current fridge's temperature to the one wanted
        public void SetFridgeTemperature(Temperature targetTemperature)
        {
            if (targetTemperature.Value < MIN_FRIDGE_TEMPERATURE_CELSIUS || targetTemperature.Value > MAX_FRIDGE_TEMPERATURE_CELSIUS)
            {
                return;
            }
            CurrentTemperature = targetTemperature;
            Touch();
        }

        // Overload for primitive doubling
        public void SetFridgeTemperature(double targetTemperature)
        {
            SetFridgeTemperature(new Temperature(targetTemperature));
        }

        public override string ToString()
        {
            return $"{base.ToString()}, Temp: {CurrentTemperature}";
        }
    }
}
