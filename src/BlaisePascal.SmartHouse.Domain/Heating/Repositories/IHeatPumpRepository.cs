using System;
using System.Collections.Generic;

using BlaisePascal.SmartHouse.Domain.Heating.HeatingDevices;

namespace BlaisePascal.SmartHouse.Domain.Heating.Repositories
{
    public interface IHeatPumpRepository
    {
        void Add(HeatPump heatPump);
        void Update(HeatPump heatPump);
        void Remove(Guid id);
        HeatPump? GetById(Guid id);
        List<HeatPump> GetAll();
    }
}
