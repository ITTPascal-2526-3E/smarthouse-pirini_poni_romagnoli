using System;
using BlaisePascal.SmartHouse.Domain.Heating.HeatingDevices;
using BlaisePascal.SmartHouse.Domain.Heating.Repositories;
using BlaisePascal.SmartHouse.Domain.Heating.HeatingOptions;
using BlaisePascal.SmartHouse.Domain.ValueObjects;

namespace BlaisePascal.SmartHouse.Application.Heating.Repositories.Commands
{
    public class AddThermostatCommand
    {
        private readonly IThermostatRepository _thermostatRepository;

        public AddThermostatCommand(IThermostatRepository thermostatRepository)
        {
            _thermostatRepository = thermostatRepository;
        }

        public void Execute(Temperature currentTemp, ModeOptionThermostat mode, Temperature targetTemp)
        {
            var thermostat = new Thermostat(currentTemp, mode, targetTemp);
            _thermostatRepository.Add(thermostat);
        }
    }
}
