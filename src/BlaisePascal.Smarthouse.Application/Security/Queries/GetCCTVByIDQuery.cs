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
    public class GetCCTVByIDQuery
    {
        private readonly ICCTVRepository _cctvRepository;
        public GetCCTVByIDQuery(ICCTVRepository cctvRepository)
        {
            _cctvRepository = cctvRepository;
        }
        public CCTV Execute(Guid id)
        {
            return _cctvRepository.GetById(id);
        }
 
        
    }
}

