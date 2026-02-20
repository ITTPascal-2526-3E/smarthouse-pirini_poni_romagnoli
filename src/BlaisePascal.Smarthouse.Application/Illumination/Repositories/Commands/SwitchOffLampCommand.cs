using BlaisePascal.SmartHouse.Domain.Illumination.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlaisePascal.SmartHouse.Application.Illumination.Repositories.Commands
{
    public class SwitchOffLampCommand
    {
        private readonly ILampRepository _lampRepository;

        public SwitchOffLampCommand(ILampRepository lampRepository)
        {
            _lampRepository = lampRepository;
        }

        public void Execute(Guid lampId)// L'utente mi passa l'id della lampada da spegnere
        {
            var lamp = _lampRepository.GetById(lampId);//leggo la lampada dal repository usando l'id passato dall'utente
            if (lamp != null)
            {
                lamp.ToggleOff();//spengo la lampada a livello Domain
                _lampRepository.Update(lamp);

            }
            
        }
    }
}
