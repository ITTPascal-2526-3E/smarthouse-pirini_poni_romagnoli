using BlaisePascal.SmartHouse.Domain;
using BlaisePascal.SmartHouse.Domain.@enum;
using BlaisePascal.SmartHouse.Domain.heating;
using System;
using System.Collections.Generic;

namespace BlaisePascal.SmartHouse.Domain
{
    // Represents a thermostat that controls one or more heat pumps
    public class Thermostat : Device
    {
        // Current measured room temperature
        public int CurrentTemperature { get; private set; }

        // Current operating mode of the thermostat
        public ModeOptionThermostat Mode { get; private set; }

        // Target temperature set on the thermostat
        public int TargetTemperature { get; private set; }

        // List of controlled heat pumps
        private readonly List<HeatPump> _heatPumps = new List<HeatPump>();

        // Constructor initializes the thermostat with initial temperature, mode and target
        public Thermostat(int currentTemperature, ModeOptionThermostat mode, int targetTemperature)
            : base("Unnamed Thermostat", mode != ModeOptionThermostat.Off) // If mode is not Off, Status is true
        {
            CurrentTemperature = currentTemperature;
            Mode = mode;
            TargetTemperature = targetTemperature;
            Touch();
        }

        // Adds a heat pump to the list of controlled devices if not already present
        public void AddHeatPump(HeatPump pump)
        {
            if (pump == null) return;

            if (!_heatPumps.Contains(pump))
            {
                _heatPumps.Add(pump);
                Touch();
            }
        }

        // Removes a heat pump from the list of controlled devices
        public void RemoveHeatPump(HeatPump pump)
        {
            if (pump == null) return;

            if (_heatPumps.Remove(pump))
            {
                Touch();
            }
        }

        // Updates the current measured temperature and re-evaluates control mode
        public void UpdateCurrentTemperature(int temperature)
        {
            CurrentTemperature = temperature;
            ControlMode();
        }

        // Sets the thermostat mode and propagates it to all connected heat pumps
        public void SetMode(ModeOptionThermostat mode)
        {
            Mode = mode;

            // Synchronize the parent Device status with the mode
            if (mode == ModeOptionThermostat.Off)
            {
                Status = false;
            }
            else
            {
                Status = true;
            }

            // Map thermostat mode to heat pump mode
            ModeOptionHeatPump hpMode = ModeOptionHeatPump.Off;

            switch (mode)
            {
                case ModeOptionThermostat.Heating:
                    hpMode = ModeOptionHeatPump.Heating;
                    break;

                case ModeOptionThermostat.Cooling:
                    hpMode = ModeOptionHeatPump.Cooling;
                    break;

                case ModeOptionThermostat.Off:
                    hpMode = ModeOptionHeatPump.Off;
                    break;
            }

            // Propagate mode to all connected heat pumps
            foreach (var pump in _heatPumps)
            {
                pump.SetMode(hpMode);
            }

            Touch();
        }

        // Sets target temperature and propagates it to all controlled heat pumps
        public void SetTargetTemperature(int temperature)
        {
            TargetTemperature = temperature;
            Touch();

            // Propagate desired temperature to all pumps
            foreach (var pump in _heatPumps)
            {
                pump.SetTargetTemperature(temperature);
            }

            // Re-evaluate the mode after changing the target
            ControlMode();
        }

        // Internal control method that sets mode based on current and target temperatures
        private void ControlMode()
        {
            if (CurrentTemperature < TargetTemperature)
            {
                // If CurrentTemperature < TargetTemperature -> Heating mode
                SetMode(ModeOptionThermostat.Heating);
            }
            else if (CurrentTemperature > TargetTemperature)
            {
                // If CurrentTemperature > TargetTemperature -> Cooling mode
                SetMode(ModeOptionThermostat.Cooling);
            }
            else
            {
                // If CurrentTemperature == TargetTemperature -> Off
                SetMode(ModeOptionThermostat.Off);
            }
        }
    }
}
