using System;
using System.Collections.Generic;
using BlaisePascal.SmartHouse.Domain;


public class Thermostat : Device
{
    

    public int CurrentTemperature { get; private set; }
    public ModeOptionThermostat Mode { get; private set; }
    public int TargetTemperature { get; private set; }

    // List of  heat pumps
    public List<HeatPump> _heatPumps = new List<HeatPump>();

    private Guid DeviceId { get; } = Guid.NewGuid();

    // RIMOSSO: private string name... -> Ora usa base.Name ereditato da Device

    // Costruttore
    public Thermostat(int currtemp, ModeOptionThermostat mod, int targtemp)
        : base("Unnamed Thermostat", mod != ModeOptionThermostat.Off) // Se mod non è Off, lo Status è true
    {
        CurrentTemperature = currtemp;
        Mode = mod;
        TargetTemperature = targtemp;
        LastmodifiedAtUtc = DateTime.Now;
    }


    public void AddHeatPump(HeatPump pump)
    {
        if (pump == null) return;

        if (!_heatPumps.Contains(pump))
        {
            _heatPumps.Add(pump);
            LastmodifiedAtUtc = DateTime.Now; // Modifica strutturale rilevante
        }
    }


    public void RemoveHeatPump(HeatPump pump)
    {
        if (pump == null) return;

        if (_heatPumps.Remove(pump))
        {
            LastmodifiedAtUtc = DateTime.Now;
        }
    }

    // Update current measured temperature
    public void updateCurrentTemp(int temp)
    {
        CurrentTemperature = temp;
        // LastmodifiedAtUtc viene aggiornato implicitamente da ControlMode -> SetMode
        // Re-evaluate mode whenever the measured temperature changes
        ControlMode();
    }

    // Manually set thermostat mode (used internally by ControlMode)
    public void SetMode(ModeOptionThermostat mode)
    {
        Mode = mode;

        // Sincronizziamo lo stato del Device genitore
        Status = (mode != ModeOptionThermostat.Off);
        LastmodifiedAtUtc = DateTime.Now;

        // Map thermostat mode -> heat pump mode
        ModeOptionheatPump hpMode = ModeOptionheatPump.Off;

        switch (mode)
        {
            case ModeOptionThermostat.Heating:
                hpMode = ModeOptionheatPump.Heating;
                break;

            case ModeOptionThermostat.Cooling:
                hpMode = ModeOptionheatPump.Cooling;
                break;

            case ModeOptionThermostat.Off:
                hpMode = ModeOptionheatPump.Off;
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
        LastmodifiedAtUtc = DateTime.Now;

        // Propagate desired temperature to all pumps (simple direct call)
        foreach (var pump in _heatPumps)
        {
            pump.SetTargetTemperature(temperature);
        }

        // Re-evaluate the mode after changing the target
        ControlMode();
    }

    // simple control method to set mode based on current and target temperatures
    private void ControlMode()
    {
        if (CurrentTemperature < TargetTemperature)// If CurrentTemperature < TargetTemperature  -> Heating
        {
            SetMode(ModeOptionThermostat.Heating);
        }
        else if (CurrentTemperature > TargetTemperature)// If CurrentTemperature > TargetTemperature  -> Cooling
        {
            SetMode(ModeOptionThermostat.Cooling);
        }
        else
        {
            SetMode(ModeOptionThermostat.Off);// If CurrentTemperature == TargetTemperature -> Off
        }
    }
}