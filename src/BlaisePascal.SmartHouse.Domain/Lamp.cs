 public class Lamp
    {

    private int luminosity_perc { get; set; }
    private int luminosity { get;  set; }
    private int power { get; }
    private string color { get; set; }
    private string model { get; }
    private bool isOn { get; set; }
    private string brand { get;  }
    string energyClass { get; }

    public Lamp(int _power,string _color, string _model, string _brand, string _energyclass)
    {
      
        luminosity_perc = 0;
        power = _power;
        color = _color;
        model = _model;
        brand = _brand;
        isOn = false;
        energyClass = _energyclass;

    }


    public void TurnOn()
    {
        isOn = true;
        luminosity_perc = 100;
    }

    public void TurnOff()
    {
        isOn = false;
        luminosity_perc = 0;
    }

    











}