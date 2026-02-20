using System;
using BlaisePascal.SmartHouse.Domain.Heating.HeatingDevices;
using BlaisePascal.SmartHouse.Domain.Heating.Repositories;
using BlaisePascal.SmartHouse.Domain.ValueObjects;

namespace BlaisePascal.SmartHouse.Application.Heating.Repositories.Commands
{
    public class UpdateHeatPumpCommand
    {
        private readonly IHeatPumpRepository _heatPumpRepository;

        public UpdateHeatPumpCommand(IHeatPumpRepository heatPumpRepository)
        {
            _heatPumpRepository = heatPumpRepository;
        }

        public void Execute(Guid id, Temperature targetTemp, Power power)
        {
            var heatPump = _heatPumpRepository.GetById(id);
            if (heatPump != null)
            {
                heatPump.SetTargetTemperature(targetTemp);
                heatPump.ChangePower(power);
                _heatPumpRepository.Update(heatPump);
            }
        }
    }
}
