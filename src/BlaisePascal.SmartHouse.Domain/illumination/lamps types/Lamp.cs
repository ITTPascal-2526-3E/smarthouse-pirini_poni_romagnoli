using BlaisePascal.SmartHouse.Domain.Abstraction;
using BlaisePascal.SmartHouse.Domain.illumination;
using BlaisePascal.SmartHouse.Domain.ValueObjects;
using System;

namespace BlaisePascal.SmartHouse.Domain.illumination
{
    // Represents a basic smart lamp device
    public class Lamp : Device, IDimmable
    {
        // Lamp power in watts
        public int Power { get; }

        // Luminosity property from IDimmable
        public Luminosity Luminosity => CurrentLuminosity;

        // Lamp color chosen from predefined options
        public ColorOption Color { get; set; }

        // Manufacturer brand
        public string Brand { get; }

        // Lamp model name or number
        public string Model { get; }

        // Energy efficiency label (e.g. A++, B, etc.)
        public EnergyClass EnergyEfficiency { get; }

        // Brightness level
        public Luminosity CurrentLuminosity { get; protected set; }

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
            CurrentLuminosity = new Luminosity(0);
            Touch(); // We consider the creation as an initial modification
        }

        // Turns the lamp ON, sets full brightness and updates the last modified timestamp
        public override void ToggleOn()
        {
            base.ToggleOn();
            CurrentLuminosity = new Luminosity(100);
            Touch();
        }

        // Turns the lamp OFF, sets brightness to zero and updates the last modified timestamp
        public override void ToggleOff()
        {
            base.ToggleOff();
            CurrentLuminosity = new Luminosity(0);
            Touch();
        }

        // Adjusts brightness if the lamp is ON and the requested value is in the valid range
        public virtual void SetLuminosity(Luminosity luminosity)//virtual allows derived classes to override this method
        {
            // Brightness can be adjusted only if the lamp is ON
            if (!IsOn)
            {
                return;
            }

            CurrentLuminosity = luminosity;
            Touch();
        }

        // Overload for convenience if needed, or forced by legacy code
        public void SetLuminosity(int value)
        {
            try
            {
                SetLuminosity(new Luminosity(value));
            }
            catch (ArgumentException)
            {
                // Ignore invalid values as per previous logic (return)
            }
        }

        public override string ToString()
        {
            return $"{base.ToString()}, Model: {Model}, Brightness: {CurrentLuminosity}";
        }
    }
}
