using BlaisePascal.SmartHouse.Domain.Abstraction;
using BlaisePascal.SmartHouse.Domain.heating;
using BlaisePascal.SmartHouse.Domain.illumination;
using BlaisePascal.SmartHouse.Domain.ValueObjects;
using System;

namespace BlaisePascal.SmartHouse.Domain.heating
{
    // Represents a smart heat pump device controlled by a thermostat
    public sealed class HeatPump : Device, ITemperatureControl, IProgrammable
    {
        // Current measured temperature
        public Temperature CurrentTemperature { get; private set; }

        // Target temperature set by the user or thermostat
        public Temperature TargetTemperature { get; private set; }

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
        public Power Power { get; private set; }

        // Airflow angle (1–90 degrees)
        public Angle Angle { get; private set; }

        // Indicates if fixed-angle mode is enabled
        public bool FixedAngleOn { get; private set; }

        // Scheduled ON time
        public DateTime? ScheduledOn { get; private set; }

        // Scheduled OFF time
        public DateTime? ScheduledOff { get; private set; }

        // Minimum allowed temperature
        private const double MIN_TEMPERATURE = 16.0;

        // Maximum allowed temperature
        private const double MAX_TEMPERATURE = 30.0;

        // Default temperature used as starting point when needed
        private const double DEFAULTE_TEMPERATURE = 20.0;


        // Constructor initializes a heat pump with basic identification and initial temperature
        public HeatPump(Temperature initialTemperature, string name = "Unnamed HeatPump", string brand = "Generic", string model = "ModelX", EnergyClass energyEfficiency = EnergyClass.A_plus_plus)
            : base(name, false) // Initializes Device with name and OFF status
        {
            CurrentTemperature = initialTemperature;
            TargetTemperature = initialTemperature;
            Brand = brand;
            Model = model;
            EnergyEfficiency = energyEfficiency;
            Power = new Power(50); // Default Power
            Angle = new Angle(45); // Default Angle
            Touch();// Initial modification timestamp
        }

        // Sets the operating mode of the heat pump, optionally turning it ON or OFF
        public void SetMode(ModeOptionHeatPump mode)
        {
            Mode = mode;

            if (mode == ModeOptionHeatPump.Off)
            {
                ToggleOff();
            }
            else
            {
                ToggleOn();
            }

            Touch();
        }

        // Sets the target temperature using internal validation logic
        public void SetTargetTemperature(Temperature temperature)
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
                CurrentTemperature = CurrentTemperature + 1.0;
            }
            else if (Mode == ModeOptionHeatPump.Cooling && CurrentTemperature > TargetTemperature)
            {
                CurrentTemperature = CurrentTemperature - 1.0;
            }

            // We do not call Touch() here because Update may be called very frequently;
        }

        // Turns the heat pump ON and updates the last modified timestamp
        public override void ToggleOn()
        {
            base.ToggleOn();
            Touch();
        }

        // Turns the heat pump OFF and updates the last modified timestamp
        public override void ToggleOff()
        {
            base.ToggleOff();
            Touch();
        }

        // Changes the visible device name and updates the last modified timestamp
        public void ChangeName(string name)
        {
            Rename(name);
        }

        // Validates and updates the target temperature
        public void ChangeTemperature(Temperature temperature)
        {
            // We use the double value for clamping, then create a new Temperature
            double clampedVal = Math.Clamp(temperature.Value, MIN_TEMPERATURE, MAX_TEMPERATURE);
            TargetTemperature = new Temperature(clampedVal);
            Touch();
        }

        // Overload for legacy support
        public void ChangeTemperature(int temperature)
        {
            ChangeTemperature(new Temperature(temperature));
        }

        // Returns the current target temperature
        public Temperature GetTemperature()
        {
            return TargetTemperature;
        }

        // Validates and updates the power level
        public void ChangePower(Power power)
        {
            Power = power;
            Touch();
        }

        // Overload for legacy support
        public void ChangePower(int power)
        {
            try
            {
                Power = new Power(power);
                Touch();
            }
            catch (ArgumentException)
            {
                // Ignore invalid values (fail silent as per previous Clamp behavior implies valid-only, but clamping is different than ignoring. 
                // Previous code: Power = Clamp(power, MIN_POW, MAX_POW); -> This forced the value into range.
                // New behavior with VO: We can either clamp here OR throw.
                // To maintain equivalent behavior to "Clamp", we should clamp the int before creating the VO.
                int clamped = Math.Clamp(power, Power.MinValue, Power.MaxValue);
                Power = new Power(clamped);
                Touch();
            }
        }

        // Returns the current power level
        public Power GetPower()
        {
            return Power;
        }


        // Activates fixed-angle airflow mode and sets the default angle
        public void SetFixedAngle()
        {
            FixedAngleOn = true;
            Angle = new Angle(45); // Default Angle
            Touch();
        }

        // Validates and updates the airflow angle
        public void ChangeAngle(Angle angle)
        {
            Angle = angle;
            Touch();
        }

        // Overload for legacy support
        public void ChangeAngle(int angle)
        {
            // Maintain clamping behavior
            int clamped = Math.Clamp(angle, Angle.MinValue, Angle.MaxValue);
            Angle = new Angle(clamped);
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
        public void Update(DateTime now)
        {
            if (ScheduledOn.HasValue && now >= ScheduledOn.Value)
            {
                ToggleOn();
                ScheduledOn = null;
            }

            if (ScheduledOff.HasValue && now >= ScheduledOff.Value)
            {
                ToggleOff();
                ScheduledOff = null;
            }

            Touch();
        }

    }
}
