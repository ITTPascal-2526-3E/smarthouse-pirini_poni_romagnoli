using System;
using System.Collections.Generic;

using BlaisePascal.SmartHouse.Domain.Illumination.LampTypes;
 
 namespace BlaisePascal.SmartHouse.Domain.Illumination.Repositories
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
