using System;

public class AirConditioner
{
    private Guid Id { get; } // Unique identifier for the air conditioner
    public string Name { get; set; } // Name assigned by the user
    public string Brand { get; set; } // The brand of the air conditioner
    public string Model { get; set; } // The model of the air conditioner
    public enum EnergyClass { A_plus_plus_plus , A_plus_plus, A_plus, A, B, C, D } // Energy efficiency standard classes
    public EnergyClass EnergyEfficency; // The energy efficiency class of the air conditioner
    public bool IsOn { get; protected set; } // True if the air conditioner is ON
    public int Temperature { get; set; } // The temperature setting of the air conditioner(16-30 degrees Celsius)
    public int Power { get; set; } // The power level of the air conditioner (0-100)
    public int Angolation { get; set; } // The angle of the air conditioner (1-90 degrees)
    public bool FixedAngleOn { get; protected set; } // True if the fixed angle mode is On
    public DateTime? ScheduledOn { get; private set; } // Scheduled time to turn ON
    public DateTime? ScheduledOff { get; private set; } // Scheduled time to turn OFF
    
    private const int DEAFULT_TEMP = 20; // Default temperature
    private const int MIN_TEMP = 16; // Minimum temperature
    private const int MAX_TEMP = 30; // Maximum temperature
    private const int DEFAULT_POW = 50; // Default power
    private const int MIN_POW = 0; // Minimum power
    private const int MAX_POW = 100; // Maximum power
    private const int DEFAULT_ANGLE = 45; // Default angolation for fixed angle mode
    private const int MIN_ANGLE = 1; // Minimum angolation
    private const int MAX_ANGLE = 90; // Maximum angolation
    private const int DEAFULT_INCREASE_POW = 5; // Default increase power by button press



    public AirConditioner(string name, string brand, string model, EnergyClass energyEfficency)
    {
        Name = name;
        IsOn = false;
        Id = Guid.NewGuid();
        Temperature = DEAFULT_TEMP;
        Power = DEFAULT_POW;
        FixedAngleOn = false;
        Brand = brand;
        Model = model;
        EnergyEfficency = energyEfficency;
    }

    // Turn the air conditioner ON
    public void TurnOn()
    {
        IsOn = true;
    }

    // Turn the air conditioner OFF
    public void TurnOff()
    {
        IsOn = false;
    }

    // Change the name of the air conditioner
    public void ChangeName(string name)
    {
        Name = name;
    }

    // Change the temperature setting
    public void ChangeTemperature(int temperature)
    {
        if (temperature >= MIN_TEMP || temperature <= MAX_TEMP)
        {
            Temperature = temperature;
        }
        else
        {
            if (temperature > MAX_TEMP)
            {
                Temperature = MAX_TEMP;
            }
            else
            {
                Temperature = MIN_TEMP;
            }
        }
    }

    // Get the current temperature setting
    public int GetTemperature()
    {
        return Temperature;
    }

    public void ChangePower(int power)
    {
        if (power >= MIN_POW || power <= MAX_POW)
        {
            Power = power;
        }
        else
        {
            if (power > MAX_POW)
            {
                Power = MAX_POW;
            }
            else
            {
                Power = MIN_POW;
            }
        }
    }

    // Get the current power level
    public int GetPower()
    {
        return Power;
    }

    // Increase power by 5 for each button press
    public void IncreasePowerByButtonPerFive()
    {
     
    }

    // Set fixed angle mode to ON and angolation to 45 degrees
    public void SetFixedAngle()
    {
        FixedAngleOn = true;
        Angolation = DEFAULT_ANGLE;

    }

    // Change the angolation of the air conditioner
    public void ChangeAngolation(int angle)
    {
        if (angle >= MIN_ANGLE || angle <= MAX_ANGLE)
        {
            Angolation = angle;
        }
        else
        {
            if (angle > MAX_ANGLE)
            {
                Angolation = MAX_ANGLE;
            }
            else
            {
                Angolation = MIN_ANGLE;
            }
        }
    }

    // Schedule ON/OFF times
    public void Schedule(DateTime? onTime, DateTime? offTime)
    {
        ScheduledOn = onTime;
        ScheduledOff = offTime;
    }

    // Update the schedule based on the current time
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
