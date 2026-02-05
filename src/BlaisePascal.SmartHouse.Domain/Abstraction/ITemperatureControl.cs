using System;
using BlaisePascal.SmartHouse.Domain.ValueObjects;

namespace BlaisePascal.SmartHouse.Domain.Abstraction
{
    public interface ITemperatureControl
    {
        Temperature CurrentTemperature { get; }
        Temperature TargetTemperature { get; }
        void SetTargetTemperature(Temperature temperature);
    }
}
