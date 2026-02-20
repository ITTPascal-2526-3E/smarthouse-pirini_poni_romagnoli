using System;
using BlaisePascal.SmartHouse.Domain.Heating.HeatingDevices;
using BlaisePascal.SmartHouse.Domain.Heating.Repositories;

namespace BlaisePascal.SmartHouse.Application.Heating.Repositories.Queries
{
    public class GetThermostatByIDQuery
    {
        private readonly IThermostatRepository _thermostatRepository;

        public GetThermostatByIDQuery(IThermostatRepository thermostatRepository)
        {
            _thermostatRepository = thermostatRepository;
        }

        public Thermostat? Execute(Guid id)
        {
            return _thermostatRepository.GetById(id);
        }
    }
}
