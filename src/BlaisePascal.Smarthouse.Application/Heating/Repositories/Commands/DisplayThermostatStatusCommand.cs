using System;
using BlaisePascal.SmartHouse.Domain.Heating.Repositories;

namespace BlaisePascal.SmartHouse.Application.Heating.Repositories.Commands
{
    public class DisplayThermostatStatusCommand
    {
        private readonly IThermostatRepository _thermostatRepository;

        public DisplayThermostatStatusCommand(IThermostatRepository thermostatRepository)
        {
            _thermostatRepository = thermostatRepository;
        }

        public string? Execute(Guid id)
        {
            var thermostat = _thermostatRepository.GetById(id);
            if (thermostat == null) return null;

            return $"Name: {thermostat.Name}\n" +
                   $"Status: {(thermostat.Status ? "ON" : "OFF")}\n" +
                   $"Mode: {thermostat.Mode}\n" +
                   $"Current Temperature: {thermostat.CurrentTemperature}\n" +
                   $"Target Temperature: {thermostat.TargetTemperature}";
        }
    }
}
