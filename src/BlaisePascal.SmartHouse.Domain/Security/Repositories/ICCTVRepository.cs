using System;
using System.Collections.Generic;

using BlaisePascal.SmartHouse.Domain.Security.SecurityDevices;

namespace BlaisePascal.SmartHouse.Domain.Security.Repositories
{
    public interface ICCTVRepository
    {
        void Add(CCTV cctv);
        void Update(CCTV cctv);
        void Remove(Guid id);
        CCTV? GetById(Guid id);
        List<CCTV> GetAll();
    }
}
