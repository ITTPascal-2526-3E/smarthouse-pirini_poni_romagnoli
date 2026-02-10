using BlaisePascal.SmartHouse.Domain.Abstraction;
using BlaisePascal.SmartHouse.Domain.Illumination.LampOptions;

namespace BlaisePascal.SmartHouse.Domain.Food
{
    public sealed class CoffeeMachine : Device, IProgrammable
    {
        public string Brand { get; set; }
        public string Model { get; set; }
        public EnergyClass EnergyEfficiency; // Fixed spelling
        public bool IsReady { get; set; } // True if the coffee machine is ready to use
        public DateTime IgnitionTime { get; set; } // Time of ignition
        public DateTime ShutdownTime { get; set; } // Time of shutdown
        public DateTime? ScheduledOn { get; private set; } // Scheduled time to turn ON
        public DateTime? ScheduledOff { get; private set; } // Scheduled time to turn OFF       

        public CoffeeMachine(string name, string brand, string model, EnergyClass energyEfficiency, bool status) : base(name, true)
        {
            Brand = brand;
            Model = model;
            EnergyEfficiency = energyEfficiency;
            IsReady = false;
        }

        //Turns on the machine
        public override void ToggleOn()
        {
            base.ToggleOn();
            IsReady = true;
            IgnitionTime = DateTime.UtcNow;
        }
        //Turns off the machine
        public override void ToggleOff()
        {
            base.ToggleOff();
            IsReady = false;
            ShutdownTime = DateTime.UtcNow;
        }

        //Schedules the machine to turn on and off at specific times  
        public void Schedule(DateTime? onTime, DateTime? offTime)
        {
            ScheduledOn = onTime;
            ScheduledOff = offTime;
            Touch();
        }

        // Checks the current time against scheduled times and updates the machine's status accordingly
        public void Update(DateTime now)
        {
            if (ScheduledOn.HasValue && now >= ScheduledOn.Value && !Status)
            {
                ToggleOn();
                ScheduledOn = null; // Clear the scheduled time after execution
            }
            if (ScheduledOff.HasValue && now >= ScheduledOff.Value && Status)
            {
                ToggleOff();
                ScheduledOff = null; // Clear the scheduled time after execution
            }
        }
    }
}
