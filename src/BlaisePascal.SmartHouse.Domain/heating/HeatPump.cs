using System;

public class HeatPump
{
    // Operating modes used also by Thermostat
    public enum ModeOption { Heating, Cooling, Fan, Dry, Off }

    // Energy efficiency classes
    public enum EnergyClass { A_plus_plus_plus, A_plus_plus, A_plus, A, B, C, D }

    // Current measured temperature
    public int CurrentTemperature { get; private set; }

    // Target temperature set by the user or thermostat
    public int TargetTemperature { get; private set; }

    // Current operating mode
    public ModeOption Mode { get; private set; }

    // User-defined name
    public string Name { get; private set; }

    // Brand and model identification
    public string Brand { get; set; }
    public string Model { get; set; }

    // Energy efficiency rating
    public EnergyClass EnergyEfficency { get; set; }

    // Unique device identifier
    private Guid DeviceId { get; } = Guid.NewGuid();

    // Whether the device is currently ON
    public bool IsOn { get; private set; }

    // Fan/power intensity (0–100)
    public int Power { get; private set; }

    // Airflow angle (1–90 degrees)
    public int Angolation { get; private set; }

    // Indicates if fixed-angle mode is enabled
    public bool FixedAngleOn { get; private set; }

    // Scheduled ON / OFF times
    public DateTime? ScheduledOn { get; private set; }
    public DateTime? ScheduledOff { get; private set; }

    // Operational limits
    private const int MIN_TEMP = 16;
    private const int MAX_TEMP = 30;
    private const int DEFAULT_TEMP = 20;

    private const int MIN_POW = 0;
    private const int MAX_POW = 100;
    private const int DEFAULT_POW = 50;

    private const int MIN_ANGLE = 1;
    private const int MAX_ANGLE = 90;
    private const int DEFAULT_ANGLE = 45;

    private const int DEFAULT_INCREASE_POW = 5;

    // Constructor to initialize base parameters
    public HeatPump(
        int initialTemperature,
        string name = "Unnamed HeatPump",
        string brand = "Generic",
        string model = "ModelX",
        EnergyClass energyEfficency = EnergyClass.A_plus_plus)
    {
        CurrentTemperature = initialTemperature;
        TargetTemperature = initialTemperature;
        Name = name;
        Brand = brand;
        Model = model;
        EnergyEfficency = energyEfficency;

        IsOn = false;
        Power = DEFAULT_POW;
        Angolation = DEFAULT_ANGLE;
    }

    // Sets the operating mode (called by the thermostat)
    public void SetMode(ModeOption mode)
    {
        Mode = mode;
        if (mode == ModeOption.Off) TurnOff();
        else TurnOn();
    }

    // Sets the target temperature (called by the thermostat)
    public void SetTargetTemperature(int temperature)
    {
        ChangeTemperature(temperature);
    }

    // Simulates device behavior and temperature progression
    public void Update()
    {
        if (!IsOn) return;

        if (Mode == ModeOption.Heating && CurrentTemperature < TargetTemperature)
            CurrentTemperature++;

        else if (Mode == ModeOption.Cooling && CurrentTemperature > TargetTemperature)
            CurrentTemperature--;
    }

    // Turns the device ON
    public void TurnOn()
    {
        IsOn = true;
    }

    // Turns the device OFF
    public void TurnOff()
    {
        IsOn = false;
    }

    // Changes the visible device name
    public void ChangeName(string name)
    {
        Name = name;
    }

    // Validates and updates the target temperature
    public void ChangeTemperature(int temperature)
    {
        if (temperature >= MIN_TEMP && temperature <= MAX_TEMP)
            TargetTemperature = temperature;
        else
            TargetTemperature = (temperature > MAX_TEMP) ? MAX_TEMP : MIN_TEMP;// if out of range, set to nearest limit
    }

    // Returns the current target temperature
    public int GetTemperature()
    {
        return TargetTemperature;
    }

    // Validates and updates the power level
    public void ChangePower(int power)
    {
        if (power >= MIN_POW && power <= MAX_POW)
            Power = power;
        else
            Power = (power > MAX_POW) ? MAX_POW : MIN_POW;// if out of range, set to nearest limit
    }

    // Returns the current power level
    public int GetPower()
    {
        return Power;
    }

    // Increases power by a predefined step
    public void IncreasePowerByButtonPerFive()
    {
        ChangePower(Power + DEFAULT_INCREASE_POW);
    }

    // Activates fixed-angle airflow mode
    public void SetFixedAngle()
    {
        FixedAngleOn = true;
        Angolation = DEFAULT_ANGLE;
    }

    // Validates and updates the airflow angle
    public void ChangeAngolation(int angle)
    {
        if (angle >= MIN_ANGLE && angle <= MAX_ANGLE)
            Angolation = angle;
        else
            Angolation = (angle > MAX_ANGLE) ? MAX_ANGLE : MIN_ANGLE;// if out of range, set to nearest limit
    }

    // Schedules ON and OFF times
    public void Schedule(DateTime? onTime, DateTime? offTime)
    {
        ScheduledOn = onTime;
        ScheduledOff = offTime;
    }

    // Verifies if scheduled events should trigger
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
    }
}
