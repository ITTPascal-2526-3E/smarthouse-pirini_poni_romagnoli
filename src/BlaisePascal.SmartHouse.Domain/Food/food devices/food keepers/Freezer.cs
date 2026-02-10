using BlaisePascal.SmartHouse.Domain.Abstraction;
using BlaisePascal.SmartHouse.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlaisePascal.SmartHouse.Domain.Food
{
    public class Freezer : Device
    {
        public string Brand { get; private set; }
        public string Model { get; private set; }
        public int CapacityLiters { get; private set; }


        // temperature
        public Temperature CurrentTemperature { get; private set; }
        public const double STANDARD_TEMPERATURE_CELSIUS = -18.0;
        public const double MIN_TEMPERATURE_CELSIUS = -24.0;
        public const double MAX_TEMPERATURE_CELSIUS = -6.0;


        // indicate if the freezer's door is open
        public bool IsDoorOpen { get; private set; }
        public bool IsLightOn { get; private set; }

        public Freezer(string brand, string model, int capacityLiters, string name)
            : base(name, true) // Freezer usually starts ON
        {
            Brand = brand;
            Model = model;
            CapacityLiters = capacityLiters;
            CurrentTemperature = new Temperature(STANDARD_TEMPERATURE_CELSIUS); // Default freezer temperature
            IsDoorOpen = false;
            IsLightOn = false;
            Touch();
        }

        // Opens the freezer door
        public override void ToggleOn()
        {
            IsDoorOpen = true;
            IsLightOn = true;
            Touch();
        }
        public override void ToggleOff()
        {
            IsDoorOpen = false;
            IsLightOn = false;
            Touch();
        }

        // Overloaded method to set freezer temperature explicitly
        public void SetFreezerTemperature(Temperature targetTemperature)
        {
            if (targetTemperature.Value < MIN_TEMPERATURE_CELSIUS || targetTemperature.Value > MAX_TEMPERATURE_CELSIUS)
            {
                return;
            }
            CurrentTemperature = targetTemperature;
            Touch();
        }

        // Overload for primitive doubling
        public void SetFreezerTemperature(double targetTemperature)
        {
            SetFreezerTemperature(new Temperature(targetTemperature));
        }

        public override string ToString()
        {
            return $"{base.ToString()}, Temp: {CurrentTemperature}";
        }
    }
}
