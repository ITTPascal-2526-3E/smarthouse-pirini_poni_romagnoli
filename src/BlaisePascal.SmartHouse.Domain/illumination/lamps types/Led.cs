using System;
using BlaisePascal.SmartHouse.Domain.illumination;
using BlaisePascal.SmartHouse.Domain.Abstraction;
using BlaisePascal.SmartHouse.Domain.ValueObjects;

namespace BlaisePascal.SmartHouse.Domain.illumination
{
    public sealed class Led : Device
    {
        public ColorOption colorOption { get; private set; }
        public Luminosity LightIntensity { get; private set; } = new Luminosity(DEFAULT_INTENSITY);

        private const int DEFAULT_INTENSITY = 70;

        public Led(string name, bool status, ColorOption color)
            : base(name, status)
        {
            colorOption = color;
        }

        public void ChangeColor(ColorOption colOption)
        {
            colorOption = colOption;
        }

        public void SetLightIntensity(int intensity)
        {
            LightIntensity = new Luminosity(intensity);
        }
    }
}
