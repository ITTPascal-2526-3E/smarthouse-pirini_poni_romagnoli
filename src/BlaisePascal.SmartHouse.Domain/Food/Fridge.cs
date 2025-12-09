using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public bool lightOn { get; private set; }

        // Fridge temperatures
        // Current temperature in Celsius
        public double CurrentFridgeTemperatureCelsius { get; private set; }
        // Minimum temperature in Celsius for fridge
        public double MinFridgeTemperatureCelsius { get; } = 2.0;
        // Maximum temperature in Celsius for fridge
        public double MaxFridgeTemperatureCelsius { get; } = 6.0;

        //Freezer temperatures 
        // Current freezer temperature in Celsius
        public double CurrentFreezerTemperatureCelsius { get; private set; }
        // Minimum temperature in Celsius for freezer
        public double MinFreezerTemperatureCelsius { get; } = -18.0;
        // Maximum temperature in Celsius for freezer
        public double MaxFreezerTemperatureCelsius { get; } = -12.0;

        // Indicates if the fridge door is open
        public bool IsFridgeDoorOpen { get; private set; }
        public bool isFreezerDoorOpen { get; private set; }
        public Fridge(string brand, string model, int capacityLiters, string name)
            : base(name, false)
        {
            Brand = brand;
            Model = model;
            CapacityLiters = capacityLiters;

            CurrentFridgeTemperatureCelsius = 4.0; // Default fridge temperature

            CurrentFreezerTemperatureCelsius = -15.0; // Default freezer temperature

            IsFridgeDoorOpen = false;
            lightOn = false;
            isFreezerDoorOpen = false;
            Touch();
        }
        //Opens the fridge door
        public void openFrdigeDoor()
        {
            IsFridgeDoorOpen = true;
            lightOn = true;
            Touch();
        }
        //Closes the fridge door
        public void closeFridgeDoor()
        {
            IsFridgeDoorOpen = false;
            lightOn = false;
            Touch();
        }
        //Opens the freezer door
        public void openFreezerDoor()
        {
            isFreezerDoorOpen = true;
            Touch();
        }
        //Closes the freezer door
        public void closeFreezerDoor()
        {
            isFreezerDoorOpen = false;
            Touch();
        }
        //Sets current fridge's temperature to the one wanted
        public void setTemperatureFridge(double targetTemperature)
        { 
            if(targetTemperature < MinFridgeTemperatureCelsius || targetTemperature > MaxFridgeTemperatureCelsius)
            {
                return;
            }
            CurrentFridgeTemperatureCelsius = targetTemperature;
        }
        //Sets current freezer's temperature to the one wanted
        public void setTemperatureFreezer(double targetTemperature)
        {
            if (targetTemperature < MinFreezerTemperatureCelsius || targetTemperature > MaxFreezerTemperatureCelsius)
            {
                return;
            }
            CurrentFreezerTemperatureCelsius = targetTemperature;
        }

    }
}
