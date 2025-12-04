internal class Program
{
    static void Main(string[] args)
    {
        LampsRow device = new LampsRow();

        Console.WriteLine("Adding lamps");
        device.AddLamp(new Lamp(60, BlaisePascal.SmartHouse.Domain.illumination.ColorOption.WarmWhite, "ModelX", "BrandY", BlaisePascal.SmartHouse.Domain.illumination.EnergyClass.A_plus_plus, "Lamp Soggiorno"));
        device.AddLamp(new Lamp(40, BlaisePascal.SmartHouse.Domain.illumination.ColorOption.White, "ModelA", "BrandZ", BlaisePascal.SmartHouse.Domain.illumination.EnergyClass.A_plus_plus, "Lamp Smart Soggiorno"));
        Console.WriteLine($"Total lamps: {device.GetLampsCount()}");

        Console.WriteLine("\nTurning on all lamps");
        device.TurnOnAllLamps();
        Console.WriteLine($"Lamps ON: {device.GetONLampsCount()}");

        Console.WriteLine("\nSetting global luminosity to 50%");
        device.SetLuminosityAllLamps(50);

        Console.WriteLine("\nTurning off lamp at index 0");
        device.TurnOffLampAtIndex(0);
        Console.WriteLine($"Lamps ON: {device.GetONLampsCount()}");

        Console.WriteLine("\nTurning on lamp at index 0 and setting index 1 to 20%");
        device.TurnOnLampAtIndex(0);
        device.SetLuminosityAtIndex(1, 20);

        Console.WriteLine("\nRemoving lamp at index 0");
        device.RemoveLampAtIndex(0);
        Console.WriteLine($"Total lamps: {device.GetLampsCount()}");

        Console.WriteLine("\nTesting EcoLamp schedule and presence");
        device.RegisterPresenceAllEcoLamps();

        DateTime onTime = DateTime.Now.AddSeconds(1);
        DateTime offTime = DateTime.Now.AddMinutes(2);
        device.ScheduleAllEcoLamps(onTime, offTime);

        DateTime later = DateTime.Now.AddMinutes(6);
        device.UpdateAllEcoLamps(later);

        Console.WriteLine("\nClearing all lamps.");
        device.ClearAllLamps();
        Console.WriteLine($"Total lamps: {device.GetLampsCount()}");

        // CCTV
        CCTV camera = new CCTV("ModelC", "BrandD", "1080p", 5, 2, "Front Door Camera", false);
        camera.StartRecording();
        camera.zoom(3);
        camera.StopRecording();
        camera.StartRecording();
        camera.ToggleNightVision();
        camera.StopRecording();

        // thermostat + heat pump 

        Console.WriteLine("\nThermostat + HeatPump test");

        HeatPump pump1 = new HeatPump(20, "Living Room Pump");
        HeatPump pump2 = new HeatPump(18, "Bedroom Pump");

        Thermostat thermostat = new Thermostat(20, BlaisePascal.SmartHouse.Domain.ModeOptionThermostat.Off, 22);
        thermostat.AddHeatPump(pump1);
        thermostat.AddHeatPump(pump2);

        thermostat.SetTargetTemperature(22);

        thermostat.updateCurrentTemp(19); // Heating
        Console.WriteLine($"Mode after temp=19: {thermostat.Mode}");

        thermostat.updateCurrentTemp(24); // Cooling
        Console.WriteLine($"Mode after temp=24: {thermostat.Mode}");

        thermostat.updateCurrentTemp(22); // Off
        Console.WriteLine($"Mode after temp=22: {thermostat.Mode}");
    }
}
