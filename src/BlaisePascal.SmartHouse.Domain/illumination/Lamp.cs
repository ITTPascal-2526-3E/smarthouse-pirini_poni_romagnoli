using BlaisePascal.SmartHouse.Domain.Abstraction;
using BlaisePascal.SmartHouse.Domain.illumination;
using System;

namespace BlaisePascal.SmartHouse.Domain
{
    // Represents a basic smart lamp device
    public class Lamp : Device
    {
        // Minimum allowed luminosity percentage
        public const int MinLuminosity = 0;

        // Maximum allowed luminosity percentage
        public const int MaxLuminosity = 100;

        // Lamp power in watts
        public int Power { get; }


        // Lamp color chosen from predefined options
        public ColorOption Color { get; set; }

        // Manufacturer brand
        public string Brand { get; }

        // Lamp model name or number
        public string Model { get; }

        // Energy efficiency label (e.g. A++, B, etc.)
        public EnergyClass EnergyEfficiency { get; }

        // Brightness level (0–100%)
        public int LuminosityPercentage { get; protected set; }

        // Indicates if the lamp is currently ON (mapped to the base Status)
        public bool IsOn => Status;

        // Constructor initializes lamp properties and sets initial state to OFF
        public Lamp(int power, ColorOption color, string model, string brand, EnergyClass energyClass, string name)
            : base(name, false)
        {
            Power = power;
            Color = color;
            Model = model;
            Brand = brand;
            EnergyEfficiency = energyClass;
            LuminosityPercentage = MinLuminosity;
            Touch(); // We consider the creation as an initial modification
        }

        // Turns the lamp ON, sets full brightness and updates the last modified timestamp
        public override void ToggleOn()
        {
            base.ToggleOn();
            LuminosityPercentage = MaxLuminosity;
            Touch();
        }

        // Turns the lamp OFF, sets brightness to zero and updates the last modified timestamp
        public override void ToggleOff()
        {
            base.ToggleOff();
            LuminosityPercentage = MinLuminosity;
            Touch();
        }

        // Adjusts brightness if the lamp is ON and the requested value is in the valid range
        public virtual void SetLuminosity(int percentage)//virtual allows derived classes to override this method
        {
            // Brightness can be adjusted only if the lamp is ON
            if (!IsOn)
            {
                return;
            }

            // Reject values outside of the allowed range
            if (percentage < MinLuminosity || percentage > MaxLuminosity)
            {
                return;
            }

            LuminosityPercentage = percentage;
            Touch();
        }
    }
}
