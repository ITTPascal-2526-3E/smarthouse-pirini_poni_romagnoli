using System;
using System.Collections.Generic;
using System.Linq;
using BlaisePascal.SmartHouse.Domain.Food.Repositories;
using BlaisePascal.SmartHouse.Domain.Food;

namespace BlaisePascal.SmartHouse.Infrastructure.Repositories.Devices.Food
{
    public class InMemoryRefrigeratorRepository : IRefrigeratorRepository
    {
        private readonly List<Refrigerator> _refrigerators = new List<Refrigerator>();

        public void Add(Refrigerator refrigerator)
        {
            _refrigerators.Add(refrigerator);
        }

        public void Remove(Guid id)
        {
            var refrigerator = _refrigerators.FirstOrDefault(r => r.DeviceId == id);
            if (refrigerator != null)
            {
                _refrigerators.Remove(refrigerator);
            }
        }

        public void Update(Refrigerator refrigerator)
        {
            var index = _refrigerators.FindIndex(r => r.DeviceId == refrigerator.DeviceId);
            if (index != -1)
            {
                _refrigerators[index] = refrigerator;
            }
        }

        public Refrigerator? GetById(Guid id)
        {
            return _refrigerators.FirstOrDefault(r => r.DeviceId == id);
        }

        public List<Refrigerator> GetAll()
        {
            return _refrigerators.ToList();
        }
    }
}
