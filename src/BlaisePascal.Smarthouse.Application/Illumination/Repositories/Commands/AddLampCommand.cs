using System;
using BlaisePascal.SmartHouse.Domain.Illumination.Repositories;
using BlaisePascal.SmartHouse.Domain.Illumination.LampTypes;
using BlaisePascal.SmartHouse.Domain.Illumination.LampOptions;

namespace BlaisePascal.SmartHouse.Application.Illumination.Repositories.Commands
{
    public class AddLampCommand
    {
        private readonly ILampRepository _lampRepository;

        public AddLampCommand(ILampRepository lampRepository)
        {
            _lampRepository = lampRepository;
        }

        public void Execute(int power, ColorOption color, string model, string brand, EnergyClass energyClass, string name)
        {
            var lamp = new Lamp(power, color, model, brand, energyClass, name);
            _lampRepository.Add(lamp);
        }
    }
}