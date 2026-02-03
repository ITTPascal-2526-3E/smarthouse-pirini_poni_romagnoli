using BlaisePascal.SmartHouse.Domain.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlaisePascal.SmartHouse.Domain
{
    public class Freezer : Device 
    {
        public string Brand { get; private set; }
        public string Model { get; private set; }
        public int CapacityLiters { get; private set; }
        public double CurrentTemperatureCelsius { get; private set; }
        public const double STANDARD_TEMPERATURE = -18.0;
        public const double MIN_TEMPERATURE = -24.0;
        public const double MAX_TEMPERATURE = -6.0;
        public bool IsDoorOpen { get; private set; }



    }
}
