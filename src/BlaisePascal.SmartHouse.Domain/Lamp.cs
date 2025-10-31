public class Lamp
{
    
    public int LuminosityPercentage { get; private set; }
    public int Luminosity;
    public int Power { get; }
    public string Color { get; set; }
    //public int SerialNumber { get; }
    public string Model { get; }
    public bool IsOn { get; private set; }
    public string Brand { get; }
    public string EnergyClass { get; }

    private int mela = 0;
    
    public Lamp(int power, string color, string model, string brand, string energyClass, int serialNumber)
    {
        Power = power;
        Color = color;
        Model = model;
        Brand = brand;
        EnergyClass = energyClass;
        IsOn = false;
        LuminosityPercentage = 0;
        //SerialNumber = serialNumber;
    }

    
    public void TurnOn()
    {
        IsOn = true;
        LuminosityPercentage = 100;
    }

    public void TurnOff()
    {
        IsOn = false;
        LuminosityPercentage = 0;
    }

    public void SetLuminosity(int percentage)
    {
        if (!IsOn)
        {
            Console.WriteLine("Accendi prima la lampada!");
           
        }

        if (percentage < 0 || percentage > 100)
        {
            Console.WriteLine("Luminosità deve essere tra 0 e 100.");

        }

        LuminosityPercentage = percentage;
        Luminosity = CalcLuminosity(); // Aggiorna il valore interno
    }

    private int CalcLuminosity()
    {
        return (Power * LuminosityPercentage) / 100;
    }

}
