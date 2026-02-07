using System;
using System.Collections.Generic;

namespace BlaisePascal.SmartHouse.Domain.illumination.Repositories
{
    public interface ILampRepository
    {
        void Add(Lamp lamp);
        void Update(Lamp lamp);
        void Remove(Guid id);
        Lamp? GetById(Guid id);
        List<Lamp> GetAll();
    }
}
