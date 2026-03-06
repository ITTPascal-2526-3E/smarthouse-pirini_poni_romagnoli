using System;
using System.Collections.Generic;
using System.Linq;
using BlaisePascal.SmartHouse.Domain.Heating.Repositories;
using BlaisePascal.SmartHouse.Domain.Heating.HeatingDevices;

namespace BlaisePascal.SmartHouse.Infrastructure.Repositories.Devices.Heating
{
    public class InMemoryHeatPumpRepository : IHeatPumpRepository
    {
        private readonly List<HeatPump> _heatPumps = new List<HeatPump>();

        public void Add(HeatPump heatPump)
        {
            _heatPumps.Add(heatPump);
        }

        public void Remove(Guid id)
        {
            var heatPump = _heatPumps.FirstOrDefault(h => h.DeviceId == id);
            if (heatPump != null)
            {
                _heatPumps.Remove(heatPump);
            }
        }

        public void Update(HeatPump heatPump)
        {
            var index = _heatPumps.FindIndex(h => h.DeviceId == heatPump.DeviceId);
            if (index != -1)
            {
                _heatPumps[index] = heatPump;
            }
        }

        public HeatPump? GetById(Guid id)
        {
            return _heatPumps.FirstOrDefault(h => h.DeviceId == id);
        }

        public List<HeatPump> GetAll()
        {
            return _heatPumps.ToList();
        }
    }
}
