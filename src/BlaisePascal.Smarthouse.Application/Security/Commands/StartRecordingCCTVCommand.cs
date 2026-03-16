using BlaisePascal.SmartHouse.Domain.Illumination.Repositories;
using BlaisePascal.SmartHouse.Domain.Security.Repositories;
using BlaisePascal.SmartHouse.Domain.Security.SecurityDevices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlaisePascal.SmartHouse.Application.Security.Commands
{
    public class StartRecordingCCTVCommand
    {
        private readonly ICCTVRepository _cctvRepository;
        public StartRecordingCCTVCommand(ICCTVRepository cctvRepository)
        {
            _cctvRepository = cctvRepository;
        }
        public void Execute(Guid cctvId)// L'utente mi passa l'id della videocamera da accendere
        {
            var cctv = _cctvRepository.GetById(cctvId);//leggo la videocamera dal repository usando l'id passato dall'utente
            if (cctv != null)
            {
                cctv.StartRecording();//accendo la videocamera a livello Domain
                _cctvRepository.Update(cctv);
            }
        }
    }
}

