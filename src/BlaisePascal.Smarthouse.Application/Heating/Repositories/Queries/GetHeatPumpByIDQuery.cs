using System;
using BlaisePascal.SmartHouse.Domain.Heating.HeatingDevices;
using BlaisePascal.SmartHouse.Domain.Heating.Repositories;

namespace BlaisePascal.SmartHouse.Application.Heating.Repositories.Queries
{
    public class GetHeatPumpByIDQuery
    {
        private readonly IHeatPumpRepository _heatPumpRepository;

        public GetHeatPumpByIDQuery(IHeatPumpRepository heatPumpRepository)
        {
            _heatPumpRepository = heatPumpRepository;
        }

        public HeatPump? Execute(Guid id)
        {
            return _heatPumpRepository.GetById(id);
        }
    }
}
