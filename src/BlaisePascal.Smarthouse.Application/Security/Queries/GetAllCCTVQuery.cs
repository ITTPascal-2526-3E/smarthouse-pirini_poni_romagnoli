using BlaisePascal.SmartHouse.Domain.Illumination.Repositories;
using BlaisePascal.SmartHouse.Domain.Security.Repositories;
using BlaisePascal.SmartHouse.Domain.Security.SecurityDevices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlaisePascal.SmartHouse.Application.Security.Queries
{
    public class GetAllCCTVQuery
    {
        private readonly ICCTVRepository _cctvRepository;
        public GetAllCCTVQuery(ICCTVRepository cctvRepository)
        {
            _cctvRepository = cctvRepository;
        }
        public IEnumerable<CCTV> Execute()
        {
            return _cctvRepository.GetAll();
        }
    }
}

