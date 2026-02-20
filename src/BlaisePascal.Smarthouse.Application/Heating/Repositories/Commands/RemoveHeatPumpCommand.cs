using System;
using BlaisePascal.SmartHouse.Domain.Heating.Repositories;

namespace BlaisePascal.SmartHouse.Application.Heating.Repositories.Commands
{
    public class RemoveHeatPumpCommand
    {
        private readonly IHeatPumpRepository _heatPumpRepository;

        public RemoveHeatPumpCommand(IHeatPumpRepository heatPumpRepository)
        {
            _heatPumpRepository = heatPumpRepository;
        }

        public void Execute(Guid id)
        {
            _heatPumpRepository.Remove(id);
        }
    }
}
