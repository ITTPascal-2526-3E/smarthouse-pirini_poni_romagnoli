using System;
using System.Collections.Generic;
using BlaisePascal.SmartHouse.Domain.Heating.HeatingDevices;
using BlaisePascal.SmartHouse.Domain.Heating.Repositories;

namespace BlaisePascal.SmartHouse.Application.Heating.Repositories.Queries
{
    public class GetAllHeatPumpsQuery
    {
        private readonly IHeatPumpRepository _heatPumpRepository;

        public GetAllHeatPumpsQuery(IHeatPumpRepository heatPumpRepository)
        {
            _heatPumpRepository = heatPumpRepository;
        }

        public List<HeatPump> Execute()
        {
            return _heatPumpRepository.GetAll();
        }
    }
}
