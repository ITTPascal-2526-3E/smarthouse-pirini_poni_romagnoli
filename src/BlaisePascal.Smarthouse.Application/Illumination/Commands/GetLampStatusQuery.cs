using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlaisePascal.SmartHouse.Domain.Heating.Repositories;
using BlaisePascal.SmartHouse.Domain.Illumination.Repositories;

namespace BlaisePascal.SmartHouse.Application.Illumination.Commands
{
    public class GetLampStatusQuery
    {
        private readonly ILampRepository _lampRepository;

        public GetLampStatusQuery(ILampRepository lampRepository)
        {
            _lampRepository = lampRepository;
        }

        public string? Execute(Guid id)
        {
            var lamp = _lampRepository.GetById(id);
            if (lamp == null) return null;

            return $"Name: {lamp.Name}\n" +
                   $"Status: {(lamp.Status ? "ON" : "OFF")}\n" +
                   $"Brightness: {lamp.Luminosity}";
        }
    }
}

