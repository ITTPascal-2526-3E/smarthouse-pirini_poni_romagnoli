using BlaisePascal.SmartHouse.Domain.illumination;
using System;

public class Lamp
{
    //  Enumeration of available lamp colors 
    

    //  Main properties 
    public int Power { get; }                  // Lamp power in watts
    public ColorOption Color { get; set; }     // Lamp color (chosen from predefined options)
    public string Brand { get; }               // Manufacturer brand
    public string Model { get; }               // Lamp model name or number
    public string EnergyClass { get; }         // Energy efficiency label (e.g. A++, B, etc.)
    public bool IsOn { get; protected set; }   // True if the lamp is currently ON
    public int LuminosityPercentage { get; protected set; } // Brightness level (0–100%)

    public Guid LampId { get; } = Guid.NewGuid(); // Unique identifier for the lamp
    public string name { get; set; } = "Unnamed Lamp"; // Optional name for easier identification

    public DateTime CreatedAtUTC { get; }

    //  Constructor 
    public Lamp(int power, ColorOption color, string model, string brand, string energyClass, string nm)
    {

        Power = power;
        Color = color;
        Model = model;
        Brand = brand;
        EnergyClass = energyClass;
        IsOn = false;
        LuminosityPercentage = 0;
        name = nm;
        CreatedAtUTC = DateTime.Now;
    }

    //  Turn lamp ON 
    public virtual void TurnOn()
    {
        IsOn = true;
        LuminosityPercentage = 100;
        
    }

    //  Turn lamp OFF 
    public virtual void TurnOff()
    {
        IsOn = false;
        LuminosityPercentage = 0;
        
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
       
    }



}