using System;
using BlaisePascal.SmartHouse.Domain.Heating.Repositories;
using BlaisePascal.SmartHouse.Domain.ValueObjects;

namespace BlaisePascal.SmartHouse.Application.Heating.Repositories.Queries
{
    public class GetThermostatActualTemperatureQuery
    {
        private readonly IThermostatRepository _thermostatRepository;

        public GetThermostatActualTemperatureQuery(IThermostatRepository thermostatRepository)
        {
            _thermostatRepository = thermostatRepository;
        }

        public Temperature? Execute(Guid id)
        {
            var thermostat = _thermostatRepository.GetById(id);
            return thermostat?.CurrentTemperature;
        }
    }
}
