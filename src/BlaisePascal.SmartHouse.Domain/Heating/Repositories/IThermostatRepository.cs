using System;
using System.Collections.Generic;

using BlaisePascal.SmartHouse.Domain.Heating.HeatingDevices;

namespace BlaisePascal.SmartHouse.Domain.Heating.Repositories
{
    public interface IThermostatRepository
    {
        void Add(Thermostat thermostat);
        void Update(Thermostat thermostat);
        void Remove(Guid id);
        Thermostat? GetById(Guid id);
        List<Thermostat> GetAll();
    }
}
