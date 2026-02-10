using System;

using BlaisePascal.SmartHouse.Domain.Abstraction;
using BlaisePascal.SmartHouse.Domain.ValueObjects;
using BlaisePascal.SmartHouse.Domain.Illumination.LampOptions;

namespace BlaisePascal.SmartHouse.Domain.Illumination.LampTypes
{
    public sealed class Led : Lamp
    {
        public Led(int id, ColorOption color, string model, string brand, EnergyClass energyClass)
            : base(10, color, model, brand, energyClass, $"Led_{id}")
        {
        }

        public void ChangeColor(ColorOption colOption)
        {
            Color = colOption;
        }

        public void SetLightIntensity(int intensity)
        {
            SetLuminosity(new Luminosity(intensity));
        }
    }
}
