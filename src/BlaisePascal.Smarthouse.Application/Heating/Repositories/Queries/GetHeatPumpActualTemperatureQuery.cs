using System;
using BlaisePascal.SmartHouse.Domain.Heating.Repositories;
using BlaisePascal.SmartHouse.Domain.ValueObjects;

namespace BlaisePascal.SmartHouse.Application.Heating.Repositories.Queries
{
    public class GetHeatPumpActualTemperatureQuery
    {
        private readonly IHeatPumpRepository _heatPumpRepository;

        public GetHeatPumpActualTemperatureQuery(IHeatPumpRepository heatPumpRepository)
        {
            _heatPumpRepository = heatPumpRepository;
        }

        public Temperature? Execute(Guid id)
        {
            var heatPump = _heatPumpRepository.GetById(id);
            return heatPump?.CurrentTemperature;
        }
    }
}
