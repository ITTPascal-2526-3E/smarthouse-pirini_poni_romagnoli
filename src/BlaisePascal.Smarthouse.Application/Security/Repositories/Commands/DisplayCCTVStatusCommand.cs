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
    public class DisplayCCTVStatusCommand
    {         
        private readonly ICCTVRepository _cctvRepository;
        public DisplayCCTVStatusCommand(ICCTVRepository cctvRepository)
        {
            _cctvRepository = cctvRepository;
        }
        public string Execute(Guid id)
        {
            var cctv = _cctvRepository.GetById(id);
            if (cctv != null)
            {
                return $"CCTV ID: {cctv.DeviceId}, Name: {cctv.Name}, IsRecording: {cctv.IsRecording}";
            }
            return "CCTV not found.";
        }
    }
}
