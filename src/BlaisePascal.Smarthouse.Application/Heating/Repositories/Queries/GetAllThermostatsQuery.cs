using System;
using System.Collections.Generic;
using BlaisePascal.SmartHouse.Domain.Heating.HeatingDevices;
using BlaisePascal.SmartHouse.Domain.Heating.Repositories;

namespace BlaisePascal.SmartHouse.Application.Heating.Repositories.Queries
{
    public class GetAllThermostatsQuery
    {
        private readonly IThermostatRepository _thermostatRepository;

        public GetAllThermostatsQuery(IThermostatRepository thermostatRepository)
        {
            _thermostatRepository = thermostatRepository;
        }

        public List<Thermostat> Execute()
        {
            return _thermostatRepository.GetAll();
        }
    }
}
