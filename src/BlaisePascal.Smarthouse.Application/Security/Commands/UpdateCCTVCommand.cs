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
    public class UpdateCCTVCommand
    {         
        private readonly ICCTVRepository _cctvRepository;
        public UpdateCCTVCommand(ICCTVRepository cctvRepository)
        {
            _cctvRepository = cctvRepository;
        }
        public void Execute(Guid id)
        {
            var cctv = _cctvRepository.GetById(id);
            if (cctv != null)
            {
                
                _cctvRepository.Update(cctv);
            }
        }
    }
}

