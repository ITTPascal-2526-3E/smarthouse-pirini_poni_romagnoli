using System;

namespace BlaisePascal.SmartHouse.Domain.Abstraction
{
    public interface IProgrammable
    {
        void Schedule(DateTime? onTime, DateTime? offTime);
        void Update(DateTime now);
    }
}
