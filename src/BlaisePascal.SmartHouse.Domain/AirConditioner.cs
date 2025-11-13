using System;

/*public class AirConditioner
{
    private Guid Id { get; }
    public string Name { get; set; }
    public string Brand { get; set; }
    public string Model { get; set; }
    public enum EnergyClass { A+++ , A++ , A+ , A, B, C, D }
    public EnergyClass EnergyEfficency;
    public bool IsOn { get; protected set; }
    public int Temperature { get; set; }
    public int Power { get; set; }
    public int Angolation { get; set; }
    public bool FixedAngleOn { get; protected set; }
    public DateTime? ScheduledOn { get; private set; }
    public DateTime? ScheduledOff { get; private set; }

    public AirConditioner(string name, string brand, string model, EnergyClass energyEfficency)
    {
        Name = name;
        IsOn = false;
        Id = Guid.NewGuid();
        Temperature = 20;
        Power = 50;
        FixedAngleOn = false;
        Brand = brand;
        Model = model;
        EnergyEfficency = EnergyClass;
    }

    public void TurnOn()
    {
        IsOn = true;
    }

    public void TurnOff()
    {
        IsOn = false;
    }

    public void ChangeName(string name)
    {
        Name = name;
    }

    public void ChangeTemperature(int temperature)
    {
        if (temperature >= 16 || temperature <= 30)
        {
            Temperature = temperature;
        }
        else
        {
            if (temperature > 30)
            {
                Temperature = 30;
            }
            else
            {
                Temperature = 16;
            }
        }
    }

    public int GetTemperature()
    {
        return Temperature;
    }

    public void ChangePower(int power)
    {
        if (power >= 0 || power <= 100)
        {
            Power = power;
        }
        else
        {
            if (power > 100)
            {
                Power = 100;
            }
            else
            {
                Power = 0;
            }
        }
    }

    public int GetPower()
    {
        return Power;
    }

    public void IncreasePowerByButtonPerFive(string button)
    {

    }

    public void SetFixedAngle()
    {
        FixedAngleOn = true;
        Angolation = 45;

    }

    public void ChangeAngolation(int angle)
    {
        if (angle >= 1 || angle <= 90)
        {
            Angolation = angle;
        }
        else
        {
            if (angle > 90)
            {
                Angolation = 90;
            }
            else
            {
                Angolation = 1;
            }
        }
    }

    public void Schedule(DateTime? onTime, DateTime? offTime)
    {
        ScheduledOn = onTime;
        ScheduledOff = offTime;
    }

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

}*/
