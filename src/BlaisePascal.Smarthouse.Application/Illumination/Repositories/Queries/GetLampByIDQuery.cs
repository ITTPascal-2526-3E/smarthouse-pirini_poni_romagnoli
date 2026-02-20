using BlaisePascal.SmartHouse.Domain.Illumination.LampTypes;
using BlaisePascal.SmartHouse.Domain.Illumination.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlaisePascal.SmartHouse.Application.Illumination.Repositories.Queries
{
    public class GetLampByIDQuery
    {

        private readonly ILampRepository _lampRepository;

        public GetLampByIDQuery(ILampRepository lampRepository)
        {
            _lampRepository = lampRepository;
        }

        public Lamp? Execute(Guid lampId)// L'utente mi passa l'id della lampada da leggere
        {
            var lamp = _lampRepository.GetById(lampId);//leggo la lampada dal repository usando l'id passato dall'utente
            return lamp;
        }

    }
                
}
