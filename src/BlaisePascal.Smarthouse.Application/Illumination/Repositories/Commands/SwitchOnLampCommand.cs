using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlaisePascal.SmartHouse.Domain.Illumination.Repositories;

namespace BlaisePascal.SmartHouse.Application.Illumination.Repositories.Commands
{
    public class SwitchOnLampCommand
    {
        private readonly ILampRepository _lampRepository;

        public SwitchOnLampCommand(ILampRepository lampRepository)
        {
            _lampRepository = lampRepository;
        }

        public void Execute(Guid lampId)
        {
            var lamp = _lampRepository.GetById(lampId);
            if (lamp != null)
            {
                lamp.ToggleOn();
                _lampRepository.Update(lamp);

            }

        }
    }
}
