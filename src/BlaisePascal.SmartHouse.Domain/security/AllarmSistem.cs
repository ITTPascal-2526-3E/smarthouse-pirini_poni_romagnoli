using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlaisePascal.SmartHouse.Domain.security
{
    class AllarmSistem
    {
        private string Barand { get; set; }
        private string Model { get; set; }
        public bool Signal { get; private set; }

        public AllarmSistem( string barand, string model)
        {
            Barand = barand;
            Model = model;
            Signal = false;
        }

        public void ActivateSignal()
        {
            Signal = true;
        }

        public void DeactivateSignal()
        {
            Signal = false;
        }

        public void Indicateoff()
        { 
            
        }
        



    }
}
