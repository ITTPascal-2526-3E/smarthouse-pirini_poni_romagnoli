using System;
using System.Collections.Generic;
using System.Linq;
using BlaisePascal.SmartHouse.Domain.Security.Repositories;
using BlaisePascal.SmartHouse.Domain.Security.SecurityDevices;

namespace BlaisePascal.SmartHouse.Infrastructure.Repositories.Devices.Security
{
    public class InMemoryAlarmSystemRepository : IAlarmSystemRepository
    {
        private readonly List<AlarmSystem> _alarms = new List<AlarmSystem>();

        public void Add(AlarmSystem alarmSystem)
        {
            _alarms.Add(alarmSystem);
        }

        public void Remove(Guid id)
        {
            var alarm = _alarms.FirstOrDefault(a => a.DeviceId == id);
            if (alarm != null)
            {
                _alarms.Remove(alarm);
            }
        }

        public void Update(AlarmSystem alarmSystem)
        {
            var index = _alarms.FindIndex(a => a.DeviceId == alarmSystem.DeviceId);
            if (index != -1)
            {
                _alarms[index] = alarmSystem;
            }
        }

        public AlarmSystem? GetById(Guid id)
        {
            return _alarms.FirstOrDefault(a => a.DeviceId == id);
        }

        public List<AlarmSystem> GetAll()
        {
            return _alarms.ToList();
        }
    }
}
