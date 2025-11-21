using System;

public class HeatPump
{
    // Heat pump modes
    public enum ModeOption { Heating, Cooling, Fan, Dry, Off }

    public int CurrentTemperature { get; private set; }
    public int TargetTemperature { get; private set; }
    public ModeOption Mode { get; private set; }

    public string Name { get; }
    private Guid DeviceId { get; } = Guid.NewGuid();

    public HeatPump(int initialTemperature, string name = "Unnamed HeatPump")
    {
        CurrentTemperature = initialTemperature;
        TargetTemperature = initialTemperature; // start aligned
        Mode = ModeOption.Off;
        Name = name;
    }

    // Set mode (called by the thermostat)
    public void SetMode(ModeOption mode)
    {
        Mode = mode;
    }

    // Set a target temperature requested by the thermostat
    public void SetTargetTemperature(int temperature)
    {
        TargetTemperature = temperature;
    }

    // simulate temperature changing when called
    public void Update()
    {
        if (Mode == ModeOption.Heating && CurrentTemperature < TargetTemperature)
        {
            CurrentTemperature++;
        }
        else if (Mode == ModeOption.Cooling && CurrentTemperature > TargetTemperature)
        {
            CurrentTemperature--;
        }
        // Fan and Dry may get optional modifiers in future
    }


}
