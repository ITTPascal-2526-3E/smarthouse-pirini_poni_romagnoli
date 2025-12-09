using System;

public class CofeeMachine
{
    private Guid Id { get; } // Unique identifier for the cofee machine
    public string Name { get; set; } // Name assigned by the user
    public string Brand { get; set; } // The brand of the air conditioner
    public string Model { get; set; } // The model of the air conditioner
    public enum EnergyClass { A_plus_plus_plus, A_plus_plus, A_plus, A, B, C, D } // Energy efficiency standard classes
    public EnergyClass EnergyEfficency; // The energy efficiency class of the air conditioner
    public bool IsOn { get; set; } // True if the cofee machine is on
    public bool IsReady { get; set; } // True if the cofee machine is ready to use
    public DateTime IgnitionTime { get; set; } // Time taken to heat up and be ready
    public DateTime ShutdownTime { get; set; } // Time taken to cool down after turning off
    public DateTime? ScheduledOn { get; private set; } // Scheduled time to turn ON
    public DateTime? ScheduledOff { get; private set; } // Scheduled time to turn OFF       

    public CofeeMachine(Guid id, string name, string brand, string model, EnergyClass energyEfficency)
    {
        Id = id;
        Name = name;
        Brand = brand;
        Model = model;
        EnergyEfficency = energyEfficency;
        IsOn = false;
        IsReady = false;
    }

    public void turnOn()
    { 
        IsOn = true;
        IgnitionTime = DateTime.Now;
    }

    public void turnOff()
    { 
        IsOn = false;
    }

    public void changeName(string name)
    { 
        Name = name;
    }

    public void IsReadyToUse()
    { 
        IsReady = true;
    }
}
