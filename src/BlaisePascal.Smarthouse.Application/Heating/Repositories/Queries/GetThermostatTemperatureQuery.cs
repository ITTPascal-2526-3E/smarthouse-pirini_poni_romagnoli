using System;
using BlaisePascal.SmartHouse.Domain.Heating.Repositories;
using BlaisePascal.SmartHouse.Domain.ValueObjects;

namespace BlaisePascal.SmartHouse.Application.Heating.Repositories.Queries
{
    public class GetThermostatTemperatureQuery
    {
        private readonly IThermostatRepository _thermostatRepository;

        public GetThermostatTemperatureQuery(IThermostatRepository thermostatRepository)
        {
            _thermostatRepository = thermostatRepository;
        }

        public (Temperature? Current, Temperature? Target) Execute(Guid id)// by using (Temperature? Current, Temperature? Target) we can return two values instead of just one.
        {
            var thermostat = _thermostatRepository.GetById(id);
            if (thermostat == null) return (null, null);
            return (thermostat.CurrentTemperature, thermostat.TargetTemperature);
        }
    }
}
