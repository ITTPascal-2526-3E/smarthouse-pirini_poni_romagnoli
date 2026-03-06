using System;
using System.Collections.Generic;
using System.Linq;
using BlaisePascal.SmartHouse.Domain.Security.Repositories;
using BlaisePascal.SmartHouse.Domain.Security.SecurityDevices;

namespace BlaisePascal.SmartHouse.Infrastructure.Repositories.Devices.Security
{
    public class InMemoryCCTVRepository : ICCTVRepository
    {
        private readonly List<CCTV> _cctvs = new List<CCTV>();

        public void Add(CCTV cctv)
        {
            _cctvs.Add(cctv);
        }

        public void Remove(Guid id)
        {
            var camera = _cctvs.FirstOrDefault(c => c.DeviceId == id);
            if (camera != null)
            {
                _cctvs.Remove(camera);
            }
        }

        public void Update(CCTV cctv)
        {
            var index = _cctvs.FindIndex(c => c.DeviceId == cctv.DeviceId);
            if (index != -1)
            {
                _cctvs[index] = cctv;
            }
        }

        public CCTV? GetById(Guid id)
        {
            return _cctvs.FirstOrDefault(c => c.DeviceId == id);
        }

        public void TriggerAlarm(CCTV cctv)
        {
            Update(cctv);
        }

        public List<CCTV> GetAll()
        {
            return _cctvs.ToList();
        }
    }
}
