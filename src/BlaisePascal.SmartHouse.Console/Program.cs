using BlaisePascal.SmartHouse.Domain;
using BlaisePascal.SmartHouse.Domain.Abstraction;
using BlaisePascal.SmartHouse.Domain.heating;
using BlaisePascal.SmartHouse.Domain.illumination;
using BlaisePascal.SmartHouse.Domain.security;
using BlaisePascal.SmartHouse.Domain.Food;
using BlaisePascal.SmartHouse.Domain.ValueObjects;
using System;
using System.Collections.Generic;

internal sealed class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("INITIALIZING SMART HOUSE DEVICES...");

        // --- 1. Illumination: LampsRow ---
        LampsRow lampsRow = new LampsRow();
        lampsRow.AddLamp(new Lamp(60, ColorOption.WarmWhite, "L-Living", "Philips", EnergyClass.A_plus_plus, "Living Room Lamp"));
        lampsRow.AddLamp(new EcoLamp(40, ColorOption.White, "Eco-Hall", "Osram", EnergyClass.A_plus, "Hallway EcoLamp"));

        // --- 2. Security: CCTV ---
        CCTV camera = new CCTV("Cam-Front", "Sony", "4K", 10, 1, "Front Garden Camera", false);

        // --- 3. Security: Door ---
        Door frontDoor = new Door("Main Entrance", status: false); // Closed initially

        // --- 4. Heating: Thermostat & HeatPumps ---
        HeatPump pump1 = new HeatPump(new Temperature(20), "Living Room AC");
        HeatPump pump2 = new HeatPump(new Temperature(18), "Bedroom AC");
        Thermostat thermostat = new Thermostat(new Temperature(20), ModeOptionThermostat.Off, new Temperature(22));
        thermostat.AddHeatPump(pump1);
        thermostat.AddHeatPump(pump2);

        // --- 5. Security: AlarmSystem ---
        AlarmSystem alarm = new AlarmSystem("Verisure", "V-Pro");

        // --- 6. Food: Fridge ---
        BlaisePascal.SmartHouse.Domain.Food.Fridge fridge = new BlaisePascal.SmartHouse.Domain.Food.Fridge("Samsung", "FamilyHub", 500, "Kitchen Fridge");

        List<Device> allDevices = new List<Device> { lampsRow, camera, frontDoor, pump1, pump2, thermostat, alarm, fridge };

        // Subscribe to alarm events
        Action<string, string> alarmHandler = (deviceName, message) =>
        {
            Console.WriteLine($"ALARM: {deviceName} {message}");
        };

        camera.OnAlarm += alarmHandler;
        frontDoor.OnAlarm += alarmHandler;
        alarm.OnAlarm += alarmHandler;

        bool running = true;
        Console.WriteLine("DEVICES READY.");

        while (running)
        {
            Console.Clear();
            Console.WriteLine("    SMART HOUSE CONTROL PANEL");
            Console.WriteLine("[L] Toggle All Lamps  | [+] Inc Light | [-] Dec Light");
            Console.WriteLine("[D] Open/Close Door   | [X] Lock/Unlock Door");
            Console.WriteLine("[C] Toggle CCTV Rec   | [V] Night Vision | [Z] Zoom In");
            Console.WriteLine("[H] Toggle Heating    | [T] Inc Temp     | [R] Dec Temp");
            Console.WriteLine("[A] Toggle Alarm Arm  | [/] Trigger Alarm");
            Console.WriteLine("[F] Open/Close Fridge | [G] Open/Close Freezer");
            Console.WriteLine("[S] Show Full Status  | [Q] Quit");


            // Show brief status summary
            Console.WriteLine($"Lamps: {lampsRow.GetOnLampsCount()} ON | Door: {(frontDoor.Status ? "OPEN" : "CLOSED")} ({(frontDoor.IsLocked ? "LOCKED" : "UNLOCKED")})");
            Console.WriteLine($"CCTV: {(camera.IsRecording ? "REC" : "IDLE")} | Alarm: {(alarm.IsArmed ? "ARMED" : "DISARMED")}");
            Console.WriteLine($"Thermostat: {thermostat.Mode} (Target: {thermostat.TargetTemperature})");

            Console.Write("Enter Command: ");

            var input = Console.ReadKey(true).Key;

            switch (input)
            {
                // --- Illumination ---
                case ConsoleKey.L:
                    if (lampsRow.GetOnLampsCount() > 0) lampsRow.TurnOffAllLamps();
                    else lampsRow.TurnOnAllLamps();
                    break;
                case ConsoleKey.OemPlus:
                case ConsoleKey.Add:
                    lampsRow.SetLuminosityAllLamps(100);
                    break;
                case ConsoleKey.OemMinus:
                case ConsoleKey.Subtract:
                    lampsRow.SetLuminosityAllLamps(10);
                    break;

                // --- Door ---
                case ConsoleKey.D:
                    if (frontDoor.Status) frontDoor.CloseDoor();
                    else frontDoor.OpenDoor();
                    break;
                case ConsoleKey.X:
                    if (frontDoor.IsLocked) frontDoor.UnlockDoor();
                    else frontDoor.LockDoor();
                    break;

                // --- CCTV ---
                case ConsoleKey.C:
                    if (camera.Status) camera.StopRecording();
                    else camera.StartRecording();
                    break;
                case ConsoleKey.V:
                    camera.ToggleNightVision();
                    break;
                case ConsoleKey.Z:
                    int newZoom = camera.ZoomLevel + 1;
                    if (newZoom > camera.TelephotoLevel) newZoom = 0;
                    camera.Zoom(newZoom);
                    break;

                // --- Heating ---
                case ConsoleKey.H:
                    // Cycle modes: Off -> Heating -> Cooling -> Off
                    if (thermostat.Mode == ModeOptionThermostat.Off) thermostat.SetMode(ModeOptionThermostat.Heating);
                    else if (thermostat.Mode == ModeOptionThermostat.Heating) thermostat.SetMode(ModeOptionThermostat.Cooling);
                    else thermostat.SetMode(ModeOptionThermostat.Off);
                    break;
                case ConsoleKey.T:
                    thermostat.SetTargetTemperature(thermostat.TargetTemperature + 1.0);
                    break;
                case ConsoleKey.R:
                    thermostat.SetTargetTemperature(thermostat.TargetTemperature - 1.0);
                    break;

                // --- Alarm ---
                case ConsoleKey.A:
                    if (alarm.IsArmed) alarm.Disarm();
                    else alarm.Arm();
                    break;
                case ConsoleKey.Divide:
                case ConsoleKey.Oem2: // Forward slash usually
                    if (alarm.IsArmed) alarm.TriggerAlarm();
                    break;

                // --- Fridge ---
                case ConsoleKey.F:
                    if (fridge.IsDoorOpen) fridge.ToggleOff();
                    else fridge.ToggleOn();
                    break;
                case ConsoleKey.G:
                    // See comments in previous version regarding G quirk
                    break;

                // --- System ---
                case ConsoleKey.S:
                    Console.Clear();
                    Console.WriteLine("--- FULL DEVICE STATUS ---");
                    foreach (var dev in allDevices)
                    {
                        Console.WriteLine(dev.ToString());
                    }
                    Console.WriteLine("\nPress any key to return...");
                    Console.ReadKey();
                    break;

                case ConsoleKey.Q:
                    running = false;
                    break;
            }
        }

        Console.WriteLine("Goodbye!");
    }
}
