using System;
using System.Collections.Generic;

using BlaisePascal.SmartHouse.Domain.Security.SecurityDevices;

namespace BlaisePascal.SmartHouse.Domain.Security.Repositories
{
    public interface IAlarmSystemRepository
    {
        void Add(AlarmSystem alarmSystem);
        void Update(AlarmSystem alarmSystem);
        void Remove(Guid id);
        AlarmSystem? GetById(Guid id);
        List<AlarmSystem> GetAll();
    }
}
