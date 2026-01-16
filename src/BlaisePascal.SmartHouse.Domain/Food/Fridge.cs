using BlaisePascal.SmartHouse.Domain.Abstraction;
using System;

namespace BlaisePascal.SmartHouse.Domain.Food
{
    public class Fridge : Device
    {
        // Manufacturer brand
        public string Brand { get; }
        // Fridge model name or number
        public string Model { get; }
        // Capacity in liters
        public int CapacityLiters { get; }
        
        // Indicates if the fridge light is on
        public bool LightOn { get; private set; } // Renamed to PascalCase

        // Fridge temperatures
        // Current temperature in Celsius
        public double CurrentFridgeTemperatureCelsius { get; private set; }
        public const double MinFridgeTemperatureCelsius = 2.0;
        public const double MaxFridgeTemperatureCelsius = 6.0;

        //Freezer temperatures 
        // Current freezer temperature in Celsius
        public double CurrentFreezerTemperatureCelsius { get; private set; }
        public const double MinFreezerTemperatureCelsius = -18.0;
        public const double MaxFreezerTemperatureCelsius = -12.0;

        // Indicates if the fridge door is open
        public bool IsFridgeDoorOpen { get; private set; }
        public bool IsFreezerDoorOpen { get; private set; } // Renamed to PascalCase

        public Fridge(string brand, string model, int capacityLiters, string name)
            : base(name, true) // Fridge usually starts ON
        {
            Brand = brand;
            Model = model;
            CapacityLiters = capacityLiters;

            CurrentFridgeTemperatureCelsius = 4.0; // Default fridge temperature
            CurrentFreezerTemperatureCelsius = -15.0; // Default freezer temperature

            IsFridgeDoorOpen = false;
            LightOn = false;
            IsFreezerDoorOpen = false;
            Touch();
        }

        // Opens the fridge door
        public override void ToggleOn() // Fixed typo
        {
            IsFridgeDoorOpen = true;
            LightOn = true;
            Touch();
        }

        // Closes the fridge door
        public override void ToggleOff() // Fixed typo (implied standardization)
        {
            IsFridgeDoorOpen = false;
            LightOn = false;
            Touch();
        }

        // Opens the freezer door
        public void OpenFreezerDoor()
        {
            IsFreezerDoorOpen = true;
            Touch();
        }

        // Closes the freezer door
        public void CloseFreezerDoor()
        {
            IsFreezerDoorOpen = false;
            Touch();
        }

        // Sets current fridge's temperature to the one wanted
        public void SetTemperature(double targetTemperature)
        { 
            if(targetTemperature < MinFridgeTemperatureCelsius || targetTemperature > MaxFridgeTemperatureCelsius)
            {
                return;
            }
            CurrentFridgeTemperatureCelsius = targetTemperature;
            Touch();
        }

        // Overloaded method to set freezer temperature explicitly
        public void SetTemperature(double targetTemperature, bool isFreezer)
        {
            if (!isFreezer)
            {
                SetTemperature(targetTemperature);
                return;
            }

            if (targetTemperature < MinFreezerTemperatureCelsius || targetTemperature > MaxFreezerTemperatureCelsius)
            {
                return;
            }
            CurrentFreezerTemperatureCelsius = targetTemperature;
            Touch();
        }

        public override string ToString()
        {
            return $"{base.ToString()}, Temp: {CurrentFridgeTemperatureCelsius}C, Freezer: {CurrentFreezerTemperatureCelsius}C";
        }
    }
}
