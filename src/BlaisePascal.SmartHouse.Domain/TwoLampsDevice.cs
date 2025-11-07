using System;
using System.Diagnostics.CodeAnalysis;

public class TwoLampsDevice()
{
    Lamp lamp = new Lamp(60, Lamp.ColorOption.WarmWhite, "ModelX", "BrandY", "A++");
    EcoLamp ecoLamp = new EcoLamp(8, Lamp.ColorOption.CoolWhite, "SmartEcoX", "SmartBrand", "A+++");

    public void TurnOnLamp()
    { 
        lamp.TurnOn();
    }

    public void TurnOnEcoLamp()
    {
        ecoLamp.TurnOn();
    }

    public void TurnOffLamp()
    {
        lamp.TurnOff();
    }

    public void TurnOffEcoLamp()
    {
        ecoLamp.TurnOff();
    }

    public void TurnOnAllLamp()
    { 
        lamp.TurnOn();
        ecoLamp.TurnOn();
    }

    public void TurnOffAllLamp()
    {
        lamp.TurnOff();
        ecoLamp.TurnOff();
    }

    public void SetLampLuminosity(int percentage)
    {
        lamp.SetLuminosity(percentage);
    }

    public void SetEcoLampLuminosity(int percentage)
    {
        ecoLamp.SetLuminosity(percentage);
    }






}
