using BlaisePascal.SmartHouse.Domain.Illumination.Repositories;
using BlaisePascal.SmartHouse.Domain.Security.Repositories;
using BlaisePascal.SmartHouse.Domain.Security.SecurityDevices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlaisePascal.SmartHouse.Application.Security.Repositories.Commands
{
    public class RemoveCCTVCommand
    {
        private readonly ICCTVRepository _cctvRepository;

        public RemoveCCTVCommand(ICCTVRepository cctvRepository)
        {
            _cctvRepository = cctvRepository;
        }

        public void Execute(Guid id)// L'utente passa l'id della videocamera da rimuovere
        {
            _cctvRepository.Remove(id);
        }
    }
}
