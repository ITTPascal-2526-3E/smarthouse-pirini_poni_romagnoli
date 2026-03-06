using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlaisePascal.SmartHouse.Domain.Illumination.LampOptions;
using BlaisePascal.SmartHouse.Domain.Illumination.LampTypes;
using BlaisePascal.SmartHouse.Domain.Illumination.Repositories;
using BlaisePascal.SmartHouse.Domain.ValueObjects;
namespace BlaisePascal.SmartHouse.Infrastructure.Repositories.Devices.Illumination.Lamps




{
    public class InMemoryLampRepository : ILampRepository
    {
        private readonly List<Lamp?> _lamps = new List<Lamp?>();
        public InMemoryLampRepository()
        {

        }

        public void Remove(Guid id)
        {
            var lamp = _lamps.FirstOrDefault(l => l?.DeviceId == id);
            if (lamp != null)
            {
                _lamps.Remove(lamp);
            }
        }

        public void Add(Lamp lamp)
        {
            _lamps.Add(lamp);
        }
        public void Remove(Lamp lamp)
        {
            _lamps.Remove(lamp);
        }
        public void Clear()
        {
            _lamps.Clear();

        }
        public Lamp GetById(Guid id)
        {
            return _lamps.FirstOrDefault(l => l.DeviceId == id);
        }

        public List<Lamp> GetAll()
        {
            return _lamps.Where(l => l != null).Select(l => l!).ToList();
        }
         public void Update(Lamp lamp)
        {
            var index = _lamps.FindIndex(l => l.DeviceId == lamp.DeviceId);
            if (index != -1)
            {
                _lamps[index] = lamp;
            }
        }
    }
}
