using BlaisePascal.SmartHouse.Domain.Abstraction;
using System;
using BlaisePascal.SmartHouse.Domain.illumination;
namespace BlaisePascal.SmartHouse.Domain.Food
{
    public sealed class CofeeMachine : Device, IProgrammable
    {
        private Guid Id { get; } // Unique identifier for the cofee machine
        public string Brand { get; set; } // The brand of the air conditioner
        public string Model { get; set; } // The model of the air conditioner
        public EnergyClass EnergyEfficency; // The energy efficiency class of the air conditioner
        public bool IsReady { get; set; } // True if the cofee machine is ready to use
        public DateTime IgnitionTime { get; set; } // Time of ignition
        public DateTime ShutdownTime { get; set; } // Time of shutdown
        public DateTime? ScheduledOn { get; private set; } // Scheduled time to turn ON
        public DateTime? ScheduledOff { get; private set; } // Scheduled time to turn OFF       

        public CofeeMachine(Guid id, string name, string brand, string model, EnergyClass energyEfficency, bool status) : base(name, true)
        {
            Id = id;
            Brand = brand;
            Model = model;
            EnergyEfficency = energyEfficency;
            IsReady = false;
        }

        //Turns on the machine
        public override void ToggleOn()
        {
            Status = true;
            IsReady = true;
            IgnitionTime = DateTime.UtcNow;
        }
        //Turns off the machine
        public override void ToggleOff()
        {
            Status = false;
            IsReady = false;
            ShutdownTime = DateTime.UtcNow;
        }

        //Schedules the machine to turn on and off at specific times  
        public void Schedule(DateTime? onTime, DateTime? offTime)
        {
            ScheduledOn = onTime;
            ScheduledOff = offTime;
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
