using BlaisePascal.SmartHouse.Domain.Security.SecurityAbstraction;
using BlaisePascal.SmartHouse.Domain.Abstraction;

namespace BlaisePascal.SmartHouse.Domain.Security.SecurityDevices
{
    public sealed class AlarmSystem : SecurityDevice
    {
        public string Brand { get; set; }
        public string Model { get; set; }

        public AlarmSystem(string brand, string model) : base("System1SmartHouse", true)
        {
            Brand = brand;
            Model = model;
        }

        public bool Intrusion { get; private set; }
        public bool Signal { get; private set; }

        public bool Notification { get; private set; }

        public bool IntrusionNotification { get; private set; }

        public void DetectIntrusion()
        {
            Intrusion = true;
            IntrusionNotification = true;
            Signal = true;
        }

        public void ResetIntrusion()
        {
            Intrusion = false;
            IntrusionNotification = false;
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

        public void IndicateOff()
        {
            if (base.Status == false)
            { 
                Notification = true;
            }
        }

        public void IndicateOn()
        {
            if (base.Status == true)
            {
                Notification = false;
            }
        }
        public override void TriggerAlarm()
        {
             RaiseAlarm("SIREN BLARING!");
        }
     }
}
