using System;
using BlaisePascal.SmartHouse.Domain.Heating.Repositories;

namespace BlaisePascal.SmartHouse.Application.Heating.Repositories.Commands
{
    public class RemoveThermostatCommand
    {
        private readonly IThermostatRepository _thermostatRepository;

        public RemoveThermostatCommand(IThermostatRepository thermostatRepository)
        {
            _thermostatRepository = thermostatRepository;
        }

        public void Execute(Guid id)
        {
            _thermostatRepository.Remove(id);
        }
    }
}
