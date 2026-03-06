using System;
using System.Collections.Generic;
using System.Linq;
using BlaisePascal.SmartHouse.Domain.Security.Repositories;
using BlaisePascal.SmartHouse.Domain.Security.SecurityDevices;

namespace BlaisePascal.SmartHouse.Infrastructure.Repositories.Devices.Security
{
    public class InMemoryDoorRepository : IDoorRepository
    {
        private readonly List<Door> _doors = new List<Door>();

        public void Add(Door door)
        {
            _doors.Add(door);
        }

        public void Remove(Guid id)
        {
            var door = _doors.FirstOrDefault(d => d.DeviceId == id);
            if (door != null)
            {
                _doors.Remove(door);
            }
        }

        public void Update(Door door)
        {
            var index = _doors.FindIndex(d => d.DeviceId == door.DeviceId);
            if (index != -1)
            {
                _doors[index] = door;
            }
        }

        public Door? GetById(Guid id)
        {
            return _doors.FirstOrDefault(d => d.DeviceId == id);
        }

        public List<Door> GetAll()
        {
            return _doors.ToList();
        }
    }
}
