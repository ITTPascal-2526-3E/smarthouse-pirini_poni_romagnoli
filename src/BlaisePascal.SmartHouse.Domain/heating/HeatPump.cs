using BlaisePascal.SmartHouse.Domain;
using BlaisePascal.SmartHouse.Domain.@enum;
using BlaisePascal.SmartHouse.Domain.heating;
using System;

namespace BlaisePascal.SmartHouse.Domain
{
    // Represents a smart heat pump device controlled by a thermostat
    public class HeatPump : Device
    {
        // Current measured temperature
        public int CurrentTemperature { get; private set; }

        // Target temperature set by the user or thermostat
        public int TargetTemperature { get; private set; }

        // Current operating mode of the heat pump
        public ModeOptionHeatPump Mode { get; private set; }

        // Brand identification
        public string Brand { get; set; }

        // Model identification
        public string Model { get; set; }

        // Energy efficiency rating
        public EnergyClass EnergyEfficiency { get; set; }

        // Indicates whether the device is currently ON (mapped to Status)
        public bool IsOn => Status;

        // Fan or power intensity (0–100)
        public int Power { get; private set; }

        // Airflow angle (1–90 degrees)
        public int Angle { get; private set; }

        // Indicates if fixed-angle mode is enabled
        public bool FixedAngleOn { get; private set; }

        // Scheduled ON time
        public DateTime? ScheduledOn { get; private set; }

        // Scheduled OFF time
        public DateTime? ScheduledOff { get; private set; }

        // Minimum allowed temperature
        private const int MinTemperature = 16;

        // Maximum allowed temperature
        private const int MaxTemperature = 30;

        // Default temperature used as starting point when needed
        private const int DefaultTemperature = 20;

        // Minimum allowed power level
        private const int MinPower = 0;

        // Maximum allowed power level
        private const int MaxPower = 100;

        // Default power level
        private const int DefaultPower = 50;

        // Minimum airflow angle
        private const int MinAngle = 1;

        // Maximum airflow angle
        private const int MaxAngle = 90;

        // Default airflow angle
        private const int DefaultAngle = 45;

        // Default power increase step used for button control
        private const int DefaultPowerIncreaseStep = 5;

        // Constructor initializes a heat pump with basic identification and initial temperature
        public HeatPump(int initialTemperature,string name = "Unnamed HeatPump",string brand = "Generic",string model = "ModelX", EnergyClass energyEfficiency = EnergyClass.A_plus_plus)
            : base(name, false) // Initializes Device with name and OFF status
        {
            CurrentTemperature = initialTemperature;
            TargetTemperature = initialTemperature;
            Brand = brand;
            Model = model;
            EnergyEfficiency = energyEfficiency;
            Power = DefaultPower;
            Angle = DefaultAngle;
            Touch();// Initial modification timestamp
        }

        // Sets the operating mode of the heat pump, optionally turning it ON or OFF
        public void SetMode(ModeOptionHeatPump mode)
        {
            Mode = mode;

            if (mode == ModeOptionHeatPump.Off)
            {
                TurnOff();
            }
            else
            {
                TurnOn();
            }

            Touch();
        }

        // Sets the target temperature using internal validation logic
        public void SetTargetTemperature(int temperature)
        {
            ChangeTemperature(temperature);
        }

        // Simulates device behavior and temperature progression toward the target
        public void Update()
        {
            if (!IsOn) return;

            // Simulate internal working: move current temperature toward the target
            if (Mode == ModeOptionHeatPump.Heating && CurrentTemperature < TargetTemperature)
            {
                CurrentTemperature++;
            }
            else if (Mode == ModeOptionHeatPump.Cooling && CurrentTemperature > TargetTemperature)
            {
                CurrentTemperature--;
            }

            // We do not call Touch() here because Update may be called very frequently;
            
        }

        // Turns the heat pump ON and updates the last modified timestamp
        public override void TurnOn()
        {
            base.TurnOn();
            Touch();
        }

        // Turns the heat pump OFF and updates the last modified timestamp
        public override void TurnOff()
        {
            base.TurnOff();
            Touch();
        }

        // Changes the visible device name and updates the last modified timestamp
        public void ChangeName(string name)
        {
            Rename(name);
        }

        // Validates and updates the target temperature
        public void ChangeTemperature(int temperature)
        {
            TargetTemperature = Clamp(temperature, MinTemperature, MaxTemperature);
            Touch();
        }

        // Returns the current target temperature
        public int GetTemperature()
        {
            return TargetTemperature;
        }

        // Validates and updates the power level
        public void ChangePower(int power)
        {
            Power = Clamp(power, MinPower, MaxPower);
            Touch();
        }

        // Returns the current power level
        public int GetPower()
        {
            return Power;
        }

        // Increases power by a predefined step, respecting the maximum limit
        public void IncreasePowerByButtonPerFive()
        {
            ChangePower(Power + DefaultPowerIncreaseStep);
        }

        // Activates fixed-angle airflow mode and sets the default angle
        public void SetFixedAngle()
        {
            FixedAngleOn = true;
            Angle = DefaultAngle;
            Touch();
        }

        // Validates and updates the airflow angle
        public void ChangeAngle(int angle)
        {
            Angle = Clamp(angle, MinAngle, MaxAngle);
            Touch();
        }

        // Schedules ON and OFF times for the heat pump
        public void Schedule(DateTime? onTime, DateTime? offTime)
        {
            ScheduledOn = onTime;
            ScheduledOff = offTime;
            Touch();
        }

        // Verifies if scheduled events should trigger and updates device state
        public void UpdateSchedule(DateTime now)
        {
            if (ScheduledOn.HasValue && now >= ScheduledOn.Value)
            {
                TurnOn();
                ScheduledOn = null;
            }

            if (ScheduledOff.HasValue && now >= ScheduledOff.Value)
            {
                TurnOff();
                ScheduledOff = null;
            }

            Touch();
        }

        // Helper method that constrains a value within a min and max range
        private static int Clamp(int value, int min, int max)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }
    }
}
