using System;

namespace BlaisePascal.SmartHouse.Domain.Abstraction
{
    public interface ITemperatureControl
    {
        int CurrentTemperature { get; }
        int TargetTemperature { get; }
        void SetTargetTemperature(int temperature);
    }
}
