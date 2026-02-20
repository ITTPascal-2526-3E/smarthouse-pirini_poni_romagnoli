using System;
using BlaisePascal.SmartHouse.Domain.Heating.HeatingDevices;
using BlaisePascal.SmartHouse.Domain.Heating.Repositories;
using BlaisePascal.SmartHouse.Domain.Heating.HeatingOptions;
using BlaisePascal.SmartHouse.Domain.Illumination.LampOptions;
using BlaisePascal.SmartHouse.Domain.ValueObjects;

namespace BlaisePascal.SmartHouse.Application.Heating.Repositories.Commands
{
    public class AddHeatPumpCommand
    {
        private readonly IHeatPumpRepository _heatPumpRepository;

        public AddHeatPumpCommand(IHeatPumpRepository heatPumpRepository)
        {
            _heatPumpRepository = heatPumpRepository;
        }

        public void Execute(Temperature initialTemp, string name, string brand, string model, EnergyClass energy)
        {
            var heatPump = new HeatPump(initialTemp, name, brand, model, energy);
            _heatPumpRepository.Add(heatPump);
        }
    }
}
