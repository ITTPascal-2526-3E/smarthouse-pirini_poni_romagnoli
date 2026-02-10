using System;
using BlaisePascal.SmartHouse.Domain.ValueObjects;

namespace BlaisePascal.SmartHouse.Domain.Illumination.LampAbstraction
{
    public interface IDimmable
    {
        Luminosity Luminosity { get; }
        void SetLuminosity(Luminosity value);
    }
}
