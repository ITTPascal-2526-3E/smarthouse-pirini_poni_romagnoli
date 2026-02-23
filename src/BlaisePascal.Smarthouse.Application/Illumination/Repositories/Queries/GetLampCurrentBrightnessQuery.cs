using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlaisePascal.SmartHouse.Domain.Illumination.LampOptions;
using BlaisePascal.SmartHouse.Domain.Illumination.LampTypes;
using BlaisePascal.SmartHouse.Domain.Illumination.Repositories;
using BlaisePascal.SmartHouse.Domain.ValueObjects;

namespace BlaisePascal.SmartHouse.Application.Illumination.Repositories.Queries
{
    public class GetLampCurrentBrightnessQuery
    {
        readonly ILampRepository _lampRepository;

        public GetLampCurrentBrightnessQuery(ILampRepository lampRepository)
        {
            _lampRepository = lampRepository;
        }

        public Luminosity? Execute(Guid lampId)
        {
            var lamp = _lampRepository.GetById(lampId);
            return lamp?.CurrentLuminosity;
        }
    }
}
