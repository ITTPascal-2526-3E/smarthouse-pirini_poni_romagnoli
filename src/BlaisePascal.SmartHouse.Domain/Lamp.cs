public class Lamp
{
    // Proprietà principali ---
    public int Power { get; }
    public string Color { get; set; }

    public string Brand { get; }
    public string Model { get; }
    public string EnergyClass { get; }
    public bool IsOn { get; protected set; }
    public int LuminosityPercentage { get; protected set; }


    public Lamp(int power, string color, string model, string brand, string energyClass)
    {
        Power = power;
        Color = 
        Model = model;
        Brand = brand;
        EnergyClass = energyClass;
        IsOn = false;
        LuminosityPercentage = 0;
    }

    //accensione
    public virtual void TurnOn()
    {
        IsOn = true;
        LuminosityPercentage = 100;
    }

    //spegnimento
    public virtual void TurnOff()
    {
        IsOn = false;
        LuminosityPercentage = 0;
    }

    //regolazione luminosità
    public virtual void SetLuminosity(int percentage)
    {
        if (IsOn)
        {

            LuminosityPercentage = percentage;

        }
    }

}
