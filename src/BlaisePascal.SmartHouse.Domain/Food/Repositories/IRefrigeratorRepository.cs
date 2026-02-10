using System;
using System.Collections.Generic;

namespace BlaisePascal.SmartHouse.Domain.Food.Repositories
{
    public interface IRefrigeratorRepository
    {
        void Add(Refrigerator refrigerator);
        void Update(Refrigerator refrigerator);
        void Remove(Guid id);
        Refrigerator? GetById(Guid id);
        List<Refrigerator> GetAll();
    }
}
