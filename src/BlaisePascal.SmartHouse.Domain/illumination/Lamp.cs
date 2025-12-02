using BlaisePascal.SmartHouse.Domain.illumination;
using System;

public class Lamp : Device
{
    //  Enumeration of available lamp colors 


    //  Main properties 
    public int Power { get; }                  // Lamp power in watts
    public ColorOption Color { get; set; }     // Lamp color (chosen from predefined options)
    public string Brand { get; }               // Manufacturer brand
    public string Model { get; }               // Lamp model name or number
    public EnergyClass EnergyEfficency { get; }         // Energy efficiency label (e.g. A++, B, etc.)
    public bool IsOn { get; protected set; }   // True if the lamp is currently ON
    public int LuminosityPercentage { get; protected set; } // Brightness level (0–100%)

    public Guid LampId { get; } = Guid.NewGuid(); // Unique identifier for the lamp
                                                  // name e CreatedAtUTC rimossi qui perché ereditati dalla classe Device

    //  Constructor 
    public Lamp(int power, ColorOption color, string model, string brand, EnergyClass energyClass, string nm)
    : base(nm, false)
    {

        Power = power;
        Color = color;
        Model = model;
        Brand = brand;
        EnergyEfficency = energyClass;
        IsOn = false;
        LuminosityPercentage = 0;
        // Inizializziamo anche l'ultima modifica alla creazione
        LastmodifiedAtUtc = DateTime.Now;
    }

    //  Turn lamp ON 
    public virtual void TurnOn()  // "virtual" = can be redefined in classes thtat inherit from Lamp
    {
        IsOn = true;
        Status = true; // Aggiorno lo stato del padre
        LuminosityPercentage = 100;
        LastmodifiedAtUtc = DateTime.Now; // Aggiorno il timestamp di modifica

    }

    //  Turn lamp OFF 
    public virtual void TurnOff() // "virtual" = can be redefined in classes thtat inherit from Lamp
    {
        IsOn = false;
        Status = false; // Aggiorno lo stato del padre
        LuminosityPercentage = 0;
        LastmodifiedAtUtc = DateTime.Now; // Aggiorno il timestamp di modifica

    }

    //  Adjust brightness 
    public virtual void SetLuminosity(int percentage)
    {
        // Brightness can be adjusted only if the lamp is ON
        if (!IsOn)
        {

            return;
        }

        if (percentage < 0 || percentage > 100)
        {

            return;
        }

        LuminosityPercentage = percentage;
        LastmodifiedAtUtc = DateTime.Now; // Aggiorno il timestamp di modifica

    }

}