using System;
using BlaisePascal.SmartHouse.Domain.Heating.HeatingDevices;
using BlaisePascal.SmartHouse.Domain.Heating.Repositories;
using BlaisePascal.SmartHouse.Domain.Heating.HeatingOptions;
using BlaisePascal.SmartHouse.Domain.ValueObjects;

namespace BlaisePascal.SmartHouse.Application.Heating.Repositories.Commands
{
    public class UpdateThermostatCommand
    {
        private readonly IThermostatRepository _thermostatRepository;

        public UpdateThermostatCommand(IThermostatRepository thermostatRepository)
        {
            _thermostatRepository = thermostatRepository;
        }

        public void Execute(Guid id, Temperature targetTemp, ModeOptionThermostat mode)
        {
            var thermostat = _thermostatRepository.GetById(id);
            if (thermostat != null)
            {
                thermostat.SetTargetTemperature(targetTemp);
                thermostat.SetMode(mode);
                _thermostatRepository.Update(thermostat);
            }
        }
    }
}
