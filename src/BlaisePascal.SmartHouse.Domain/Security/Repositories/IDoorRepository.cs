using System;
using System.Collections.Generic;

using BlaisePascal.SmartHouse.Domain.Security.SecurityDevices;

namespace BlaisePascal.SmartHouse.Domain.Security.Repositories
{
    public interface IDoorRepository
    {
        void Add(Door door);
        void Update(Door door);
        void Remove(Guid id);
        Door? GetById(Guid id);
        List<Door> GetAll();
    }
}
