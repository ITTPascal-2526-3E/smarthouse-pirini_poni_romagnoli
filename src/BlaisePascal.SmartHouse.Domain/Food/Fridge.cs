using BlaisePascal.SmartHouse.Domain.Abstraction;
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
        public bool LightOn { get; private set; } // Renamed to PascalCase

        // Fridge temperatures
        // Current temperature in Celsius
        public double CurrentFridgeTemperatureCelsius { get; private set; }
        public const double MIN_FRIDGE_TEMPERATURE_CELSIUS = 0.0;
        public const double MAX_FRIDGE_TEMPERATURE_CELSIUS = 6.0;
        public const double STANDARD_FRIDGE_TEMPERATURE_CELSIUS = 4.0; 
        

        // Indicates if the fridge door is open
        public bool IsFridgeDoorOpen { get; private set; }
        

        public Fridge(string brand, string model, int capacityLiters,string name)
            : base(name, true) // Fridge usually starts ON
        {
            Brand = brand;
            Model = model;
            CapacityLiters = capacityLiters;

            CurrentFridgeTemperatureCelsius = STANDARD_FRIDGE_TEMPERATURE_CELSIUS; // Default fridge temperature

            IsFridgeDoorOpen = false;
            LightOn = false;
            Touch();
        }

        // Opens the fridge door
        public override void ToggleOn() 
        {
            IsFridgeDoorOpen = true;
            LightOn = true;
            Touch();
        }

        // Closes the fridge door
        public override void ToggleOff() 
        {
            IsFridgeDoorOpen = false;
            LightOn = false;
            Touch();
        }

        // Sets current fridge's temperature to the one wanted
        public void SetFridgeTemperature(double targetTemperature)
        { 
            if(targetTemperature < MIN_FRIDGE_TEMPERATURE_CELSIUS || targetTemperature > MAX_FRIDGE_TEMPERATURE_CELSIUS)
            {
                return;
            }
            CurrentFridgeTemperatureCelsius = targetTemperature;
            Touch();
        }

        public override string ToString()
        {
            return $"{base.ToString()}, Temp: {CurrentFridgeTemperatureCelsius}C°";
        }
    }
}
