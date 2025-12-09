using BlaisePascal.SmartHouse.Domain;
using BlaisePascal.SmartHouse.Domain.heating;
using BlaisePascal.SmartHouse.Domain.illumination;
using System;

internal class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("SMART HOUSE DEMO\n");

        // LampsRow
        LampsRow row = new LampsRow();
        Console.WriteLine("LampsRow: add 2 lamps");

        row.AddLamp(new Lamp(
            60,
            ColorOption.WarmWhite,
            "ModelX",
            "BrandY",
            EnergyClass.A_plus_plus,
            "Living Room Lamp"));

        row.AddLamp(new EcoLamp(
            40,
            ColorOption.White,
            "ModelA",
            "BrandZ",
            EnergyClass.A_plus_plus,
            "Smart Living Room EcoLamp"));

        Console.WriteLine($"LampsRow: total lamps = {row.GetLampsCount()}");

        row.TurnOnAllLamps();
        Console.WriteLine($"LampsRow: turn ON all, lamps ON = {row.GetOnLampsCount()}");

        row.SetLuminosityAllLamps(50);
        Console.WriteLine("LampsRow: global luminosity = 50%");

        row.TurnOffLampAtIndex(0);
        Console.WriteLine($"LampsRow: OFF lamp [0], lamps ON = {row.GetOnLampsCount()}");

        row.TurnOnLampAtIndex(0);
        row.SetLuminosityAtIndex(1, 20);
        Console.WriteLine($"LampsRow: ON lamp [0], lamp [1] luminosity = 20%, lamps ON = {row.GetOnLampsCount()}");

        // EcoLamp scheduling + presence
        row.RegisterPresenceAllEcoLamps();
        DateTime now = DateTime.UtcNow;
        DateTime onTime = now.AddSeconds(1);
        DateTime offTime = now.AddMinutes(2);

        row.ScheduleAllEcoLamps(onTime, offTime);
        Console.WriteLine($"LampsRow: EcoLamp schedule ON={onTime}, OFF={offTime}");

        DateTime later = now.AddMinutes(6);
        row.UpdateAllEcoLamps(later);
        Console.WriteLine($"LampsRow: EcoLamp update at {later}");

        row.ClearAllLamps();
        Console.WriteLine($"LampsRow: clear all, total lamps = {row.GetLampsCount()}");

        // TwoLampsDevice
        TwoLampsDevice two = new TwoLampsDevice("Desk Lamps");
        Console.WriteLine("\nTwoLampsDevice: add 2 lamps");

        two.AddLamp(new Lamp(
            10,
            ColorOption.WarmWhite,
            "DeskModel1",
            "BrandA",
            EnergyClass.A_plus_plus_plus,
            "Left Desk Lamp"));

        two.AddLamp(new EcoLamp(
            12,
            ColorOption.CoolWhite,
            "DeskModel2",
            "BrandB",
            EnergyClass.A_plus_plus,
            "Right Desk EcoLamp"));

        Console.WriteLine($"TwoLampsDevice: total lamps = {two.GetLampsCount()}");

        two.TurnOnBothLamps();
        Console.WriteLine($"TwoLampsDevice: ON both, lamps ON = {two.GetOnLampsCount()}");

        two.SetLuminosityBothLamps(60);
        Console.WriteLine("TwoLampsDevice: both luminosity = 60%");

        DateTime ecoUpdateTime = DateTime.UtcNow.AddMinutes(10);
        two.UpdateBothEcoLamps(ecoUpdateTime);
        Console.WriteLine($"TwoLampsDevice: EcoLamp update at {ecoUpdateTime}");

        // CCTV
        Console.WriteLine("\nCCTV:");
        CCTV camera = new CCTV("ModelC", "BrandD", "1080p", 5, 2, "Front Door Camera", false);

        Console.WriteLine($"CCTV: status={(camera.Status ? "ON" : "OFF")}, zoom={camera.ZoomLevel}");
        camera.StartRecording();
        Console.WriteLine($"CCTV: StartRecording -> status={(camera.Status ? "ON" : "OFF")}, zoom={camera.ZoomLevel}");

        camera.Zoom(3);
        Console.WriteLine($"CCTV: Zoom(3) -> zoom={camera.ZoomLevel}");

        camera.StartRecording();
        camera.ToggleNightVision();
        Console.WriteLine($"CCTV: night vision={(camera.IsNightVisionOn ? "ON" : "OFF")}");

        camera.StopRecording();
        Console.WriteLine("CCTV: StopRecording");

        // Door
        Console.WriteLine("\nDoor:");
        Door frontDoor = new Door("Main Door", status: false);

        Console.WriteLine($"Door: Open={frontDoor.Status}, Locked={frontDoor.IsLocked}");
        frontDoor.OpenDoor();
        Console.WriteLine($"Door: OpenDoor() -> Open={frontDoor.Status}, Locked={frontDoor.IsLocked}");

        frontDoor.UnlockDoor();
        Console.WriteLine($"Door: UnlockDoor() -> Open={frontDoor.Status}, Locked={frontDoor.IsLocked}");

        frontDoor.OpenDoor();
        Console.WriteLine($"Door: OpenDoor() -> Open={frontDoor.Status}, Locked={frontDoor.IsLocked}");

        frontDoor.CloseDoor();
        Console.WriteLine($"Door: CloseDoor() -> Open={frontDoor.Status}, Locked={frontDoor.IsLocked}");

        frontDoor.LockDoor();
        Console.WriteLine($"Door: LockDoor() -> Open={frontDoor.Status}, Locked={frontDoor.IsLocked}");

        frontDoor.OpenDoorWithKey();
        Console.WriteLine($"Door: OpenDoorWithKey() -> Open={frontDoor.Status}, Locked={frontDoor.IsLocked}");

        frontDoor.CloseDoorWithKey();
        Console.WriteLine($"Door: CloseDoorWithKey() -> Open={frontDoor.Status}, Locked={frontDoor.IsLocked}");

        // Thermostat + HeatPump
        Console.WriteLine("\nThermostat + HeatPump:");

        HeatPump pump1 = new HeatPump(20, "Living Room Pump");
        HeatPump pump2 = new HeatPump(18, "Bedroom Pump");

        Thermostat thermostat = new Thermostat(20, ModeOptionThermostat.Off, 22);
        thermostat.AddHeatPump(pump1);
        thermostat.AddHeatPump(pump2);

        Console.WriteLine($"Thermostat: initial mode={thermostat.Mode}, target={thermostat.TargetTemperature}°C");

        thermostat.SetTargetTemperature(22);
        Console.WriteLine("Thermostat: set target=22°C");

        thermostat.UpdateCurrentTemperature(19);
        Console.WriteLine($"Thermostat: temp=19 -> mode={thermostat.Mode}");
        Console.WriteLine($"HeatPumps: pump1={pump1.Mode}, pump2={pump2.Mode}");

        thermostat.UpdateCurrentTemperature(24);
        Console.WriteLine($"Thermostat: temp=24 -> mode={thermostat.Mode}");
        Console.WriteLine($"HeatPumps: pump1={pump1.Mode}, pump2={pump2.Mode}");

        thermostat.UpdateCurrentTemperature(22);
        Console.WriteLine($"Thermostat: temp=22 -> mode={thermostat.Mode}");
        Console.WriteLine($"HeatPumps: pump1={pump1.Mode}, pump2={pump2.Mode}");

        Console.WriteLine("\nDemo completed.");
        Console.ReadKey();
    }
}
