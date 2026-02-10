using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlaisePascal.SmartHouse.Domain.security;

namespace BlaisePascal.SmartHouse.Domain
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
