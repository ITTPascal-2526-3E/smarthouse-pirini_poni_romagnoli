using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlaisePascal.SmartHouse.Domain.Illumination.Repositories;
using BlaisePascal.SmartHouse.Domain.Illumination.LampTypes;
using BlaisePascal.SmartHouse.Domain.Illumination.LampOptions;

namespace BlaisePascal.SmartHouse.Application.Illumination.Queries
{
    public class GetAllLampsQuery
    {
        private readonly ILampRepository _lampRepository;
        public GetAllLampsQuery(ILampRepository lampRepository)
        {
            _lampRepository = lampRepository;
        }
        public List<Lamp> Execute()
        {
            return _lampRepository.GetAll();
        }
    }
}

