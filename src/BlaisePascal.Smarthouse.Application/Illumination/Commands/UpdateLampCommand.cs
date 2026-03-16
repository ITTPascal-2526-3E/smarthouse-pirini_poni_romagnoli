using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlaisePascal.SmartHouse.Domain.Illumination.Repositories;

namespace BlaisePascal.SmartHouse.Application.Illumination.Commands
{
    public class UpdateLampCommand
    {
        private readonly ILampRepository _lampRepository;

        public UpdateLampCommand(ILampRepository lampRepository)
        {
            _lampRepository = lampRepository;
        }

        public void Execute(Guid lampId)
        {
            var lamp = _lampRepository.GetById(lampId);
            if (lamp != null)
            {
                _lampRepository.Update(lamp);

            }

        }
    }
}

