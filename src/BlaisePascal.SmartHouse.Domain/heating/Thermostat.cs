using System;
using System.Collections.Generic;

public class Thermostat
{
    // Thermostat modes
    public enum ModeOption { Heating, Cooling, Off }

    public int CurrentTemperature { get; private set; }
    public ModeOption Mode { get; private set; }
    public int TargetTemperature { get; private set; }

    // List of  heat pumps
    public List<HeatPump> _heatPumps = new List<HeatPump>();

    private Guid DeviceId { get; } = Guid.NewGuid();
    private string name { get; set; } = "Unnamed Thermostat";

    public Thermostat(int currtemp, ModeOption mod, int targtemp)
    {
        CurrentTemperature = currtemp;
        Mode = mod;
        TargetTemperature = targtemp;
    }

    
    public void AddHeatPump(HeatPump pump)
    {
        if (pump == null) return;

        if (!_heatPumps.Contains(pump))
            _heatPumps.Add(pump);
    }

    
    public void RemoveHeatPump(HeatPump pump)
    {
        if (pump == null) return;
        _heatPumps.Remove(pump);
    }

    // Update current measured temperature
    public void updateCurrentTemp(int temp)
    {
        CurrentTemperature = temp;
        // Re-evaluate mode whenever the measured temperature changes
        ControlMode();
    }

    // Manually set thermostat mode (used internally by ControlMode)
    public void SetMode(ModeOption mode)
    {
        Mode = mode;

        // Map thermostat mode -> heat pump mode
        HeatPump.ModeOption hpMode = HeatPump.ModeOption.Off;

        switch (mode)
        {
            case ModeOption.Heating:

                hpMode = HeatPump.ModeOption.Heating;
                break;

            case ModeOption.Cooling:


                hpMode = HeatPump.ModeOption.Cooling;
                break;

            case ModeOption.Off:

                hpMode = HeatPump.ModeOption.Off;
                break;
        }

        // Propagate mode to all connected heat pumps
        foreach (var pump in _heatPumps)
        {
            pump.SetMode(hpMode);
        }
    }

    // Set target temperature and propagate to heat pumps
    public void SetTargetTemperature(int temperature)
    {
        TargetTemperature = temperature;

        // Propagate desired temperature to all pumps (simple direct call)
        foreach (var pump in _heatPumps)
        {
            pump.SetTargetTemperature(temperature);
        }

        // Re-evaluate the mode after changing the target
        ControlMode();
    }




    //simple control method to set mode based on current and target temperatures
    private void ControlMode()
    {
        if (CurrentTemperature < TargetTemperature)// If CurrentTemperature < TargetTemperature  -> Heating
        {
            SetMode(ModeOption.Heating);
        }
        else if (CurrentTemperature > TargetTemperature)// If CurrentTemperature > TargetTemperature  -> Cooling
        {
            SetMode(ModeOption.Cooling);
        }
        else
        {
            SetMode(ModeOption.Off);// If CurrentTemperature == TargetTemperature -> Off
        }
    }
}
