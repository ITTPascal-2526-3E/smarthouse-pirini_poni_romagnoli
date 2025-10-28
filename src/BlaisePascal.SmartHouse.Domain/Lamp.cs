 public class Lamp
    {

    int luminosity { get; set; }
    int power { get; }
    string color { get; set; }
    string model { get; }
    bool isOn { get; set; }
    string brand { get;  }
    string energyClass { get; }

    public Lamp(int _luminosity, int _power,string _color, string _model, string _brand, string _energyclass)
    {
      
        luminosity = _luminosity;
        power = _power;
        color = _color;
        model = _model;
        brand = _brand;
        isOn = false;
        energyClass = _energyclass;

    }



    








}