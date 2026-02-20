using System;
using BlaisePascal.SmartHouse.Domain.Heating.Repositories;
using BlaisePascal.SmartHouse.Domain.ValueObjects;

namespace BlaisePascal.SmartHouse.Application.Heating.Repositories.Queries
{
    public class GetHeatPumpTargetTemperatureQuery
    {
        private readonly IHeatPumpRepository _heatPumpRepository;

        public GetHeatPumpTargetTemperatureQuery(IHeatPumpRepository heatPumpRepository)
        {
            _heatPumpRepository = heatPumpRepository;
        }

        public Temperature? Execute(Guid id)
        {
            var heatPump = _heatPumpRepository.GetById(id);
            return heatPump?.TargetTemperature;
        }
    }
}
