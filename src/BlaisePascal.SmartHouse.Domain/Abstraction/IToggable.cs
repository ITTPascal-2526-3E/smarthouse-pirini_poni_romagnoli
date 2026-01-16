using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlaisePascal.SmartHouse.Domain.Abstraction
{
    // Device interfaces
    public interface IToggable
    {
        void ToggleOn();
        void ToggleOff();
    }
}
