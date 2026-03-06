using System;
using System.Collections.Generic;
using System.Linq;
using BlaisePascal.SmartHouse.Domain.Heating.Repositories;
using BlaisePascal.SmartHouse.Domain.Heating.HeatingDevices;

namespace BlaisePascal.SmartHouse.Infrastructure.Repositories.Devices.Heating
{
    public class InMemoryThermostatRepository : IThermostatRepository
    {
        private readonly List<Thermostat> _thermostats = new List<Thermostat>();

        public void Add(Thermostat thermostat)
        {
            _thermostats.Add(thermostat);
        }

        public void Remove(Guid id)
        {
            var thermostat = _thermostats.FirstOrDefault(t => t.DeviceId == id);
            if (thermostat != null)
            {
                _thermostats.Remove(thermostat);
            }
        }

        public void Update(Thermostat thermostat)
        {
            var index = _thermostats.FindIndex(t => t.DeviceId == thermostat.DeviceId);
            if (index != -1)
            {
                _thermostats[index] = thermostat;
            }
        }

        public Thermostat? GetById(Guid id)
        {
            return _thermostats.FirstOrDefault(t => t.DeviceId == id);
        }

        public List<Thermostat> GetAll()
        {
            return _thermostats.ToList();
        }
    }
}
