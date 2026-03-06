
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlaisePascal.SmartHouse.Domain.Security.Repositories;
using BlaisePascal.SmartHouse.Domain.Security.SecurityDevices;

namespace BlaisePascal.SmartHouse.Application.Security.Repositories.Commands
{
    public class AddCCTVCommand
    {
        private readonly ICCTVRepository _cctvRepository;

        public AddCCTVCommand(ICCTVRepository cctvRepository)
        {
            _cctvRepository = cctvRepository;
        }

        public void Execute(string model, string brand, string resolution, int telephotoLevel, int wideAngleLevel, string name, bool status)
        {
            var cctv = new CCTV(model, brand, resolution, telephotoLevel, wideAngleLevel, name, status);
            _cctvRepository.Add(cctv);
        }
    }
}
