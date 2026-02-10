using System;
using BlaisePascal.SmartHouse.Domain.ValueObjects;

namespace BlaisePascal.SmartHouse.Domain.Abstraction
{
    public interface IDimmable
    {
        Luminosity Luminosity { get; }
        void SetLuminosity(Luminosity value);
    }
}
