using BlaisePascal.SmartHouse.Domain.Illumination.Repositories;
using BlaisePascal.SmartHouse.Domain.Security.Repositories;
using BlaisePascal.SmartHouse.Domain.Security.SecurityDevices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlaisePascal.SmartHouse.Application.Security.Repositories.Queries
{
    public class GetCCTVWideAngleLevelQuery
    {
        private readonly ICCTVRepository _cctvRepository;
        public GetCCTVWideAngleLevelQuery(ICCTVRepository cctvRepository)
        {
            _cctvRepository = cctvRepository;
        }
        public int Execute(Guid id)
        {
            var cctv = _cctvRepository.GetById(id);
            if (cctv != null)
            {
                return cctv.WideAngleLevel;
            }
            throw new Exception("CCTV not found.");
        }
    }
}