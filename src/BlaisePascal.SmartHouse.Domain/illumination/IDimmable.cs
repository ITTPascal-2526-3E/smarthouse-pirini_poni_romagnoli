using System;

namespace BlaisePascal.SmartHouse.Domain.Abstraction
{
    public interface IDimmable
    {
        int Luminosity { get; }
        void SetLuminosity(int value);
    }
}
