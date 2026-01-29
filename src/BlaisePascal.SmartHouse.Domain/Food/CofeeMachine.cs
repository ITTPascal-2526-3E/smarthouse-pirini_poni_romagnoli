using BlaisePascal.SmartHouse.Domain.Abstraction;
using System;
namespace BlaisePascal.SmartHouse.Domain.Food
{
    public sealed class CofeeMachine : Device
    {
        private Guid Id { get; } // Unique identifier for the cofee machine
        public string Brand { get; set; } // The brand of the air conditioner
        public string Model { get; set; } // The model of the air conditioner
        public enum EnergyClass { A_plus_plus_plus, A_plus_plus, A_plus, A, B, C, D } // Energy efficiency standard classes
        public EnergyClass EnergyEfficency; // The energy efficiency class of the air conditioner
        public bool IsReady { get; set; } // True if the cofee machine is ready to use
        public DateTime IgnitionTime { get; set; } // Time taken to heat up and be ready
        public DateTime ShutdownTime { get; set; } // Time taken to cool down after turning off
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
        }
        //Turns off the machine
        public override void ToggleOff()
        {
            Status = false;
            IsReady = false;
        }
    }
}
