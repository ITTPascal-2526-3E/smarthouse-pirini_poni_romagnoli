using System;using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlaisePascal.SmartHouse.Domain.security
{
     public class AllarmSistem : Device
    {
        private string Brand { get; set; }
        private string Model { get; set; }

        public bool Irruption { get; private set; }
        public bool Signal { get; private set; }

        public bool Notification { get; private set; }

        public bool IrruptionNotification { get; private set; }


        public AllarmSistem( string barand, string model) : base("Sistem1Smarthouse", true )
        {
            Brand = barand;
            Model = model;
            Irruption = false;
            Signal = false;
            Notification = false;
            IrruptionNotification = false;
        }

        public void DetectIrruption()
        {
            Irruption = true;
            IrruptionNotification = true;
            Signal = true;
        }

        public void ResetIrruption()
        {
            Irruption = false;
            IrruptionNotification = false;
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

        public void IndicateOffPerSignal()
        {
            if (Signal == false)
            {
                Notification = false;
            }
        }

        public void IndicateOnPerSignal()
        {
            if (Signal == true)
            {
                Notification = true;
            }
        }

        public void Indicateoff()
        {
            if (base.Status == false)
            { 
                Notification = true;
            }
        }

        public void Indicateon()
        {
            if (base.Status == true)
            {
                Notification = false;
            }
        }
     }
}
