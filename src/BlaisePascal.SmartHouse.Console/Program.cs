internal class Program
{
    static void Main(string[] args)
    {
        Lamp lamp = new Lamp(10, Lamp.ColorOption.WarmWhite, "EcoModelX", "EcoBrand", "A++");
        lamp.TurnOn();
        Console.WriteLine($"Lamp is on: {lamp.IsOn}, Luminosity: {lamp.LuminosityPercentage}%");
        lamp.TurnOff();
        Console.WriteLine($"Lamp is on: {lamp.IsOn}, Luminosity: {lamp.LuminosityPercentage}%");
        lamp.TurnOn();
        lamp.SetLuminosity(70);
        Console.WriteLine($"Lamp is on: {lamp.IsOn}, Luminosity: {lamp.LuminosityPercentage}%");
        Console.WriteLine($"Lamp has color: {lamp.Color}");

        EcoLamp ecoLamp = new EcoLamp(8, Lamp.ColorOption.CoolWhite, "SmartEcoX", "SmartBrand", "A+++");
        ecoLamp.TurnOn();
        Console.WriteLine($"EcoLamp is on: {ecoLamp.IsOn}, Luminosity: {ecoLamp.LuminosityPercentage}%");
        ecoLamp.TurnOff();
        ecoLamp.TurnOn();
        ecoLamp.RegisterPresence();
        //ecoLamp.Schedule(DateTime.Now.AddSeconds(5), DateTime.Now.AddSeconds(10));
        //ecoLamp.Update(DateTime.Now.AddSeconds(6));
        Console.WriteLine($"EcoLamp is on: {ecoLamp.IsOn}, Luminosity: {ecoLamp.LuminosityPercentage}%");
        Console.WriteLine($"Lamp has color: {ecoLamp.Color}");


    }
}