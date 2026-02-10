using System;
using System.Collections.Generic;
using System.Linq;
using BlaisePascal.SmartHouse.Domain.Abstraction;
using BlaisePascal.SmartHouse.Domain.Illumination.LampTypes;
using BlaisePascal.SmartHouse.Domain.Illumination.LampOptions;
using BlaisePascal.SmartHouse.Domain.Security.SecurityDevices;
using BlaisePascal.SmartHouse.Domain.Heating.HeatingDevices;
using BlaisePascal.SmartHouse.Domain.Heating.HeatingOptions;
using BlaisePascal.SmartHouse.Domain.Food;
using BlaisePascal.SmartHouse.Domain.ValueObjects;

internal sealed class Program
{
    // Devices
    static List<Lamp> _lamps = null!;
    static List<Door> _doors = null!;
    static CCTV _camera = null!;
    static AlarmSystem _alarm = null!;
    static Thermostat _thermostat = null!;
    static List<HeatPump> _pumps = null!;
    static CoffeeMachine _coffee = null!;
    static Refrigerator _fridge = null!;

    static void Main(string[] args)
    {
        InitDevices();

        bool running = true;
        while (running)
        {
            Console.Clear();
            Console.WriteLine("SMART HOUSE CONTROL");
            Console.WriteLine("-------------------");
            Console.WriteLine($"1. Illumination ({_lamps.Count} lamps)");
            Console.WriteLine($"2. Security (Alarm: {_alarm.IsArmed})");
            Console.WriteLine($"3. Heating (Thermostat: {_thermostat.Mode})");
            Console.WriteLine("4. Food & Appliances");
            Console.WriteLine("5. Full Status");
            Console.WriteLine("Q. Exit");
            Console.WriteLine("-------------------");
            Console.Write("> ");

            var key = Console.ReadKey(true).Key;
            switch (key)
            {
                case ConsoleKey.D1: IlluminationMenu(); break;
                case ConsoleKey.D2: SecurityMenu(); break;
                case ConsoleKey.D3: HeatingMenu(); break;
                case ConsoleKey.D4: FoodMenu(); break;
                case ConsoleKey.D5: ShowFullStatus(); break;
                case ConsoleKey.Q: running = false; break;
            }
        }
    }

    static void InitDevices()
    {
        _lamps = new List<Lamp>
        {
            new Lamp(60, ColorOption.WarmWhite, "L-Living", "Philips", EnergyClass.A, "Living Room Lamp"),
            new EcoLamp(40, ColorOption.NeutralWhite, "Eco-Hall", "Osram", EnergyClass.A, "Hallway EcoLamp"),
            new Led(1, ColorOption.CoolWhite, "Strip-1", "Samsung", EnergyClass.A)
        };

        _doors = new List<Door>
        {
            new Door("Front Door", false),
            new Door("Back Door", false)
        };

        _camera = new CCTV("Cam-Front", "Sony", "4K", 10, 1, "Front Garden Camera", false);
        _alarm = new AlarmSystem("Verisure", "V-Pro");

        _pumps = new List<HeatPump>
        {
            new HeatPump(new Temperature(20), "Living Room AC"),
            new HeatPump(new Temperature(18), "Bedroom AC")
        };

        _thermostat = new Thermostat(new Temperature(20), ModeOptionThermostat.Off, new Temperature(22));
        foreach (var p in _pumps) _thermostat.AddHeatPump(p);

        _coffee = new CoffeeMachine("Morning Brew", "DeLonghi", "Magnifica", EnergyClass.A, false);

        var fridge = new Fridge("Samsung", "FamilyHub", 500, "Kitchen Fridge");
        var freezer = new Freezer("Samsung", "FamilyHub", 200, "Kitchen Freezer");
        _fridge = new Refrigerator(fridge, "Kitchen Refrigerator", freezer);

        Action<string, string> handler = (name, msg) =>
        {
            Console.WriteLine($"\n[ALARM] {name}: {msg}");
            Console.Write("Press any key...");
            Console.ReadKey(true);
        };
        _camera.OnAlarm += handler;
        _alarm.OnAlarm += handler;
        foreach (var d in _doors) d.OnAlarm += handler;
    }

    static void IlluminationMenu()
    {
        bool stay = true;
        while (stay)
        {
            Console.Clear();
            Console.WriteLine("ILLUMINATION");
            Console.WriteLine("------------");
            for (int i = 0; i < _lamps.Count; i++)
            {
                var l = _lamps[i];
                string status = l.IsOn ? "ON" : "OFF";
                Console.WriteLine($"{i + 1}. [{status}] {l.Name} (Lum: {l.CurrentLuminosity.Value})");
            }
            Console.WriteLine("------------");
            Console.WriteLine("[T] Toggle lamp #");
            Console.WriteLine("[A] Turn All ON");
            Console.WriteLine("[O] Turn All OFF");
            Console.WriteLine("[L] Set luminosity");
            Console.WriteLine("[B] Back");
            Console.Write("> ");

            switch (Console.ReadKey(true).Key)
            {
                case ConsoleKey.T:
                    Console.Write("Lamp #: ");
                    if (int.TryParse(Console.ReadLine(), out int ti) && ti >= 1 && ti <= _lamps.Count)
                    {
                        if (_lamps[ti - 1].IsOn) _lamps[ti - 1].ToggleOff(); else _lamps[ti - 1].ToggleOn();
                    }
                    break;
                case ConsoleKey.A:
                    foreach (var l in _lamps) l.ToggleOn();
                    break;
                case ConsoleKey.O:
                    foreach (var l in _lamps) l.ToggleOff();
                    break;
                case ConsoleKey.L:
                    Console.Write("Lamp #: ");
                    if (int.TryParse(Console.ReadLine(), out int li) && li >= 1 && li <= _lamps.Count)
                    {
                        Console.Write("Luminosity (0-100): ");
                        if (int.TryParse(Console.ReadLine(), out int lum))
                        {
                            if (!_lamps[li - 1].IsOn) _lamps[li - 1].ToggleOn();
                            _lamps[li - 1].SetLuminosity(lum);
                        }
                    }
                    break;
                case ConsoleKey.B: stay = false; break;
            }
        }
    }

    static void SecurityMenu()
    {
        bool stay = true;
        while (stay)
        {
            Console.Clear();
            Console.WriteLine("SECURITY");
            Console.WriteLine("--------");
            Console.WriteLine("Doors:");
            for (int i = 0; i < _doors.Count; i++)
            {
                var d = _doors[i];
                string locked = d.IsLocked ? "LOCKED" : "UNLOCKED";
                string state = d.Status ? "OPEN" : "CLOSED";
                Console.WriteLine($"{i + 1}. {d.Name} [{locked}] ({state})");
            }
            Console.WriteLine($"Camera: {_camera.Name} " + (_camera.IsRecording ? "[RECORDING]" : "[IDLE]"));
            Console.WriteLine($"Alarm: {(_alarm.IsArmed ? "ARMED" : "DISARMED")}");
            Console.WriteLine("--------");
            Console.WriteLine("[D] Toggle door (open/close)");
            Console.WriteLine("[K] Lock/Unlock door");
            Console.WriteLine("[C] Toggle CCTV");
            Console.WriteLine("[V] Toggle Night Vision");
            Console.WriteLine("[A] Arm/Disarm Alarm");
            Console.WriteLine("[!] Trigger Alarm");
            Console.WriteLine("[B] Back");
            Console.Write("> ");

            switch (Console.ReadKey(true).Key)
            {
                case ConsoleKey.D:
                    Console.Write("Door #: ");
                    if (int.TryParse(Console.ReadLine(), out int di) && di >= 1 && di <= _doors.Count)
                    {
                        if (_doors[di - 1].Status) _doors[di - 1].CloseDoor(); else _doors[di - 1].OpenDoor();
                    }
                    break;
                case ConsoleKey.K:
                    Console.Write("Door #: ");
                    if (int.TryParse(Console.ReadLine(), out int ki) && ki >= 1 && ki <= _doors.Count)
                    {
                        if (_doors[ki - 1].IsLocked) _doors[ki - 1].UnlockDoor(); else _doors[ki - 1].LockDoor();
                    }
                    break;
                case ConsoleKey.C:
                    if (_camera.IsRecording) _camera.StopRecording(); else _camera.StartRecording();
                    break;
                case ConsoleKey.V:
                    _camera.ToggleNightVision();
                    break;
                case ConsoleKey.A:
                    if (_alarm.IsArmed) _alarm.Disarm(); else _alarm.Arm();
                    break;
                case ConsoleKey.D1: // '!'
                    if (_alarm.IsArmed) _alarm.TriggerAlarm();
                    break;
                case ConsoleKey.B: stay = false; break;
            }
        }
    }

    static void HeatingMenu()
    {
        bool stay = true;
        while (stay)
        {
            Console.Clear();
            Console.WriteLine("HEATING");
            Console.WriteLine("-------");
            Console.WriteLine($"Thermostat: {_thermostat.Mode}");
            Console.WriteLine($"Current Temp: {_thermostat.CurrentTemperature}");
            Console.WriteLine($"Target Temp:  {_thermostat.TargetTemperature}");
            Console.WriteLine("Heat Pumps:");
            for (int i = 0; i < _pumps.Count; i++)
            {
                var p = _pumps[i];
                string status = p.IsOn ? "ON" : "OFF";
                Console.WriteLine($"{i + 1}. [{status}] {p.Name} ({p.CurrentTemperature} -> {p.TargetTemperature})");
            }
            Console.WriteLine("-------");
            Console.WriteLine("[M] Cycle Mode");
            Console.WriteLine("[T] Set Target Temp");
            Console.WriteLine("[P] Toggle Pump");
            Console.WriteLine("[W] Set Pump Power");
            Console.WriteLine("[B] Back");
            Console.Write("> ");

            switch (Console.ReadKey(true).Key)
            {
                case ConsoleKey.M:
                    if (_thermostat.Mode == ModeOptionThermostat.Off)
                        _thermostat.SetMode(ModeOptionThermostat.Heating);
                    else if (_thermostat.Mode == ModeOptionThermostat.Heating)
                        _thermostat.SetMode(ModeOptionThermostat.Cooling);
                    else
                        _thermostat.SetMode(ModeOptionThermostat.Off);
                    break;
                case ConsoleKey.T:
                    Console.Write("Target Temp: ");
                    if (double.TryParse(Console.ReadLine(), out double t))
                        _thermostat.SetTargetTemperature(new Temperature(t));
                    break;
                case ConsoleKey.P:
                    Console.Write("Pump #: ");
                    if (int.TryParse(Console.ReadLine(), out int pi) && pi >= 1 && pi <= _pumps.Count)
                    {
                        if (_pumps[pi - 1].IsOn) _pumps[pi - 1].ToggleOff(); else _pumps[pi - 1].ToggleOn();
                    }
                    break;
                case ConsoleKey.W:
                    Console.Write("Pump #: ");
                    if (int.TryParse(Console.ReadLine(), out int wi) && wi >= 1 && wi <= _pumps.Count)
                    {
                        Console.Write("Power (0-100): ");
                        if (int.TryParse(Console.ReadLine(), out int pow))
                            _pumps[wi - 1].ChangePower(pow);
                    }
                    break;
                case ConsoleKey.B: stay = false; break;
            }
        }
    }

    static void FoodMenu()
    {
        bool stay = true;
        while (stay)
        {
            Console.Clear();
            Console.WriteLine("FOOD & APPLIANCES");
            Console.WriteLine("-----------------");
            Console.WriteLine($"Coffee Machine: {_coffee.Name} [{(_coffee.Status ? "ON" : "OFF")}]");
            Console.WriteLine($"Refrigerator: {_fridge.Name}");
            Console.WriteLine($"  Fridge:  {_fridge.MyFridge.CurrentTemperature}C {(_fridge.MyFridge.IsDoorOpen ? "(OPEN)" : "(CLOSED)")}");
            Console.WriteLine($"  Freezer: {_fridge.MyFreezer.CurrentTemperature}C {(_fridge.MyFreezer.IsDoorOpen ? "(OPEN)" : "(CLOSED)")}");
            Console.WriteLine("-----------------");
            Console.WriteLine("[C] Toggle Coffee Machine");
            Console.WriteLine("[F] Open/Close Fridge");
            Console.WriteLine("[Z] Open/Close Freezer");
            Console.WriteLine("[T] Set Fridge Temp");
            Console.WriteLine("[B] Back");
            Console.Write("> ");

            switch (Console.ReadKey(true).Key)
            {
                case ConsoleKey.C:
                    if (_coffee.Status) _coffee.ToggleOff(); else _coffee.ToggleOn();
                    break;
                case ConsoleKey.F:
                    if (_fridge.MyFridge.IsDoorOpen) _fridge.CloseFridge(); else _fridge.OpenFridge();
                    break;
                case ConsoleKey.Z:
                    if (_fridge.MyFreezer.IsDoorOpen) _fridge.CloseFreezer(); else _fridge.OpenFreezer();
                    break;
                case ConsoleKey.T:
                    Console.Write("Temp (0-6): ");
                    if (double.TryParse(Console.ReadLine(), out double ft))
                        _fridge.SetMyFridgeTemp(new Temperature(ft));
                    break;
                case ConsoleKey.B: stay = false; break;
            }
        }
    }

    static void ShowFullStatus()
    {
        Console.Clear();
        Console.WriteLine("FULL DEVICE STATUS");
        Console.WriteLine("------------------");
        PrintList("Lamps", _lamps);
        PrintList("Doors", _doors);
        Console.WriteLine($"Camera: {_camera}");
        Console.WriteLine($"Alarm: {_alarm}");
        PrintList("Heat Pumps", _pumps);
        Console.WriteLine($"Thermostat: {_thermostat}");
        Console.WriteLine($"Coffee: {_coffee}");
        Console.WriteLine($"Refrigerator: {_fridge}");
        Console.WriteLine("------------------");
        Console.WriteLine("Press any key to back...");
        Console.ReadKey(true);
    }

    static void PrintList<T>(string title, List<T> items)
    {
        Console.WriteLine($"{title}:");
        foreach (var item in items) Console.WriteLine($"  {item}");
    }
}
