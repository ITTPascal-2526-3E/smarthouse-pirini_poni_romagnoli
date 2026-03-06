using System;
using System.Linq;

// Domain
using BlaisePascal.SmartHouse.Domain.ValueObjects;
using BlaisePascal.SmartHouse.Domain.Illumination.LampOptions;
using BlaisePascal.SmartHouse.Domain.Heating.HeatingOptions;
using BlaisePascal.SmartHouse.Domain.Food;

// Infrastructure Repositories
using BlaisePascal.SmartHouse.Infrastructure.Repositories.Devices.Illumination.Lamps;
using BlaisePascal.SmartHouse.Infrastructure.Repositories.Devices.Security;
using BlaisePascal.SmartHouse.Infrastructure.Repositories.Devices.Heating;
using BlaisePascal.SmartHouse.Infrastructure.Repositories.Devices.Food;

// Application Use Cases - Illumination
using BlaisePascal.SmartHouse.Application.Illumination.Repositories.Commands;
using BlaisePascal.SmartHouse.Application.Illumination.Repositories.Queries;

// Application Use Cases - Security
using BlaisePascal.SmartHouse.Application.Security.Repositories.Commands;
using BlaisePascal.SmartHouse.Application.Security.Repositories.Queries;

// Application Use Cases - Heating
using BlaisePascal.SmartHouse.Application.Heating.Repositories.Commands;
using BlaisePascal.SmartHouse.Application.Heating.Repositories.Queries;

// Application Use Cases - Food
using BlaisePascal.Smarthouse.Application.Food.Repositories.Commands;
using BlaisePascal.Smarthouse.Application.Food.Repositories.Queries;

internal sealed class Program
{
    // Repositories
    static InMemoryLampRepository _lampRepo = new InMemoryLampRepository();
    static InMemoryDoorRepository _doorRepo = new InMemoryDoorRepository();
    static InMemoryCCTVRepository _cctvRepo = new InMemoryCCTVRepository();
    static InMemoryAlarmSystemRepository _alarmRepo = new InMemoryAlarmSystemRepository();
    static InMemoryThermostatRepository _thermostatRepo = new InMemoryThermostatRepository();
    static InMemoryHeatPumpRepository _heatPumpRepo = new InMemoryHeatPumpRepository();
    static InMemoryCoffeeMachineRepository _coffeeRepo = new InMemoryCoffeeMachineRepository();
    static InMemoryRefrigeratorRepository _fridgeRepo = new InMemoryRefrigeratorRepository();

    // Queries
    static GetAllLampsQuery _getAllLamps = new GetAllLampsQuery(_lampRepo);
    static GetAllCCTVQuery _getAllCctv = new GetAllCCTVQuery(_cctvRepo);
    static GetAllThermostatsQuery _getAllThermostats = new GetAllThermostatsQuery(_thermostatRepo);
    static GetAllHeatPumpsQuery _getAllHeatPumps = new GetAllHeatPumpsQuery(_heatPumpRepo);
    static GetAllCoffeeMachinesQuery _getAllCoffee = new GetAllCoffeeMachinesQuery(_coffeeRepo);
    static GetAllRefrigeratorsQuery _getAllFridges = new GetAllRefrigeratorsQuery(_fridgeRepo);

    // Commands
    // Illumination
    static SwitchOnLampCommand _switchOnLamp = new SwitchOnLampCommand(_lampRepo);
    static SwitchOffLampCommand _switchOffLamp = new SwitchOffLampCommand(_lampRepo);
    static UpdateLampCommand _updateLamp = new UpdateLampCommand(_lampRepo);

    // Heating
    static UpdateThermostatCommand _updateThermostat = new UpdateThermostatCommand(_thermostatRepo);
    static UpdateHeatPumpCommand _updateHeatPump = new UpdateHeatPumpCommand(_heatPumpRepo);

    // We will instantiate commands as needed to avoid too much boilerplate upfront
    static AddLampCommand _addLamp = new AddLampCommand(_lampRepo);
    
    // Security 
    static AddCCTVCommand _addCctv = new AddCCTVCommand(_cctvRepo);
    static UpdateCCTVCommand _updateCctv = new UpdateCCTVCommand(_cctvRepo);

    static void Main(string[] args)
    {
        InitData();

        bool running = true;
        while (running)
        {
            Console.Clear();
            Console.WriteLine("SMART HOUSE CONTROL (USE CASES OVER CQRS)");
            Console.WriteLine("");
            
            var lamps = _getAllLamps.Execute();
            var cctvs = _getAllCctv.Execute();
            var doors = _doorRepo.GetAll();
            var alarm = _alarmRepo.GetAll().First();
            var thermostat = _getAllThermostats.Execute().First();
            
            Console.WriteLine($"1. Illumination ({lamps.Count} lamps)");
            Console.WriteLine($"2. Security (Alarm: {alarm.IsArmed})");
            Console.WriteLine($"3. Heating (Thermostat: {thermostat.Mode})");
            Console.WriteLine("4. Food & Appliances");
            Console.WriteLine("5. Full Status");
            Console.WriteLine("Q. Exit");
            Console.WriteLine("");
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

    static void InitData()
    {
        // Illumination
        _addLamp.Execute(60, ColorOption.WarmWhite, "Philips", "Living Room Lamp", EnergyClass.A, "L-Living");
        _addLamp.Execute(40, ColorOption.NeutralWhite, "Osram", "Hallway EcoLamp", EnergyClass.A, "Eco-Hall");
        _addLamp.Execute(1, ColorOption.CoolWhite, "Samsung", "LED Strip", EnergyClass.A, "Strip-1");

        // Security
        _doorRepo.Add(new BlaisePascal.SmartHouse.Domain.Security.SecurityDevices.Door("Front Door", false));
        _doorRepo.Add(new BlaisePascal.SmartHouse.Domain.Security.SecurityDevices.Door("Back Door", false));

        var cctv = new BlaisePascal.SmartHouse.Domain.Security.SecurityDevices.CCTV("Cam-Front", "Sony", "4K", 10, 1, "Front Garden Camera", false);
        _cctvRepo.Add(cctv);

        var alarm = new BlaisePascal.SmartHouse.Domain.Security.SecurityDevices.AlarmSystem("Verisure", "V-Pro");
        _alarmRepo.Add(alarm);

        // Security Alarms subscriptions
        Action<string, string> handler = (name, msg) =>
        {
            Console.WriteLine($"\n[ALARM] {name}: {msg}");
            Console.Write("Press any key...");
            Console.ReadKey(true);
        };
        cctv.OnAlarm += handler;
        alarm.OnAlarm += handler;
        foreach (var d in _doorRepo.GetAll()) d.OnAlarm += handler;

        // Heating
        var hp1 = new BlaisePascal.SmartHouse.Domain.Heating.HeatingDevices.HeatPump(new Temperature(20), "Living Room AC");
        var hp2 = new BlaisePascal.SmartHouse.Domain.Heating.HeatingDevices.HeatPump(new Temperature(18), "Bedroom AC");
        _heatPumpRepo.Add(hp1);
        _heatPumpRepo.Add(hp2);

        var thermostat = new BlaisePascal.SmartHouse.Domain.Heating.HeatingDevices.Thermostat(new Temperature(20), ModeOptionThermostat.Off, new Temperature(22));
        var addPumpCommand = new BlaisePascal.SmartHouse.Application.Heating.Repositories.Commands.AddHeatPumpCommand(_heatPumpRepo);
        
        thermostat.AddHeatPump(hp1);
        thermostat.AddHeatPump(hp2);
        _thermostatRepo.Add(thermostat);

        // Food
        var addCoffee = new AddCoffeeMachineCommand(_coffeeRepo);
        addCoffee.Execute("Morning Brew", "DeLonghi", "Magnifica", EnergyClass.A, false);

        var fridge = new Fridge("Samsung", "FamilyHub", 500, "Kitchen Fridge");
        var freezer = new Freezer("Samsung", "FamilyHub", 200, "Kitchen Freezer");
        var addFridge = new AddRefrigeratorCommand(_fridgeRepo);
        addFridge.Execute(fridge, "Kitchen Refrigerator", freezer);
    }

    static void IlluminationMenu()
    {
        bool stay = true;
        while (stay)
        {
            Console.Clear();
            Console.WriteLine("ILLUMINATION");
            Console.WriteLine("");
            var lamps = _getAllLamps.Execute();
            
            for (int i = 0; i < lamps.Count; i++)
            {
                var l = lamps[i];
                string status = l.IsOn ? "ON" : "OFF";
                Console.WriteLine($"{i + 1}. [{status}] {l.Name} (Lum: {l.CurrentLuminosity?.Value ?? 0})");
            }
            Console.WriteLine("");
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
                    if (int.TryParse(Console.ReadLine(), out int ti) && ti >= 1 && ti <= lamps.Count)
                    {
                        var lamp = lamps[ti - 1];
                        if (lamp.IsOn) _switchOffLamp.Execute(lamp.DeviceId); else _switchOnLamp.Execute(lamp.DeviceId);
                    }
                    break;
                case ConsoleKey.A:
                    foreach (var l in lamps) _switchOnLamp.Execute(l.DeviceId);
                    break;
                case ConsoleKey.O:
                    foreach (var l in lamps) _switchOffLamp.Execute(l.DeviceId);
                    break;
                case ConsoleKey.L:
                    Console.Write("Lamp #: ");
                    if (int.TryParse(Console.ReadLine(), out int li) && li >= 1 && li <= lamps.Count)
                    {
                        Console.Write("Luminosity (0-100): ");
                        if (int.TryParse(Console.ReadLine(), out int lum))
                        {
                            var lamp = lamps[li - 1];
                            if (!lamp.IsOn) _switchOnLamp.Execute(lamp.DeviceId);
                            
                            lamp.SetLuminosity(lum); // Domain change
                            _updateLamp.Execute(lamp.DeviceId); // Persistence
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
            Console.WriteLine("");
            
            var doors = _doorRepo.GetAll();
            var camera = _getAllCctv.Execute().First();
            var alarm = _alarmRepo.GetAll().First();

            Console.WriteLine("Doors:");
            for (int i = 0; i < doors.Count; i++)
            {
                var d = doors[i];
                string locked = d.IsLocked ? "LOCKED" : "UNLOCKED";
                string state = d.Status ? "OPEN" : "CLOSED";
                Console.WriteLine($"{i + 1}. {d.Name} [{locked}] ({state})");
            }
            Console.WriteLine($"Camera: {camera.Name} " + (camera.IsRecording ? "[RECORDING]" : "[IDLE]"));
            Console.WriteLine($"Alarm: {(alarm.IsArmed ? "ARMED" : "DISARMED")}");
            Console.WriteLine("");
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
                    if (int.TryParse(Console.ReadLine(), out int di) && di >= 1 && di <= doors.Count)
                    {
                        var door = doors[di - 1];
                        if (door.Status) door.CloseDoor(); else door.OpenDoor();
                        _doorRepo.Update(door);
                    }
                    break;
                case ConsoleKey.K:
                    Console.Write("Door #: ");
                    if (int.TryParse(Console.ReadLine(), out int ki) && ki >= 1 && ki <= doors.Count)
                    {
                        var door = doors[ki - 1];
                        if (door.IsLocked) door.UnlockDoor(); else door.LockDoor();
                        _doorRepo.Update(door);
                    }
                    break;
                case ConsoleKey.C:
                    if (camera.IsRecording) camera.StopRecording(); else camera.StartRecording();
                    _updateCctv.Execute(camera.DeviceId);
                    break;
                case ConsoleKey.V:
                    camera.ToggleNightVision();
                    _updateCctv.Execute(camera.DeviceId);
                    break;
                case ConsoleKey.A:
                    if (alarm.IsArmed) alarm.Disarm(); else alarm.Arm();
                    _alarmRepo.Update(alarm);
                    break;
                case ConsoleKey.D1: // '!'
                    if (alarm.IsArmed) alarm.TriggerAlarm();
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
            Console.WriteLine("");
            
            var thermostat = _getAllThermostats.Execute().First();
            var pumps = _getAllHeatPumps.Execute();

            Console.WriteLine($"Thermostat: {thermostat.Mode}");
            Console.WriteLine($"Current Temp: {thermostat.CurrentTemperature}");
            Console.WriteLine($"Target Temp:  {thermostat.TargetTemperature}");
            Console.WriteLine("Heat Pumps:");
            for (int i = 0; i < pumps.Count; i++)
            {
                var p = pumps[i];
                string status = p.IsOn ? "ON" : "OFF";
                Console.WriteLine($"{i + 1}. [{status}] {p.Name} ({p.CurrentTemperature} -> {p.TargetTemperature})");
            }
            Console.WriteLine("");
            Console.WriteLine("[M] Cycle Mode");
            Console.WriteLine("[T] Set Target Temp");
            Console.WriteLine("[P] Toggle Pump");
            Console.WriteLine("[W] Set Pump Power");
            Console.WriteLine("[B] Back");
            Console.Write("> ");

            switch (Console.ReadKey(true).Key)
            {
                case ConsoleKey.M:
                    ModeOptionThermostat newMode = thermostat.Mode;
                    if (thermostat.Mode == ModeOptionThermostat.Off)
                        newMode = ModeOptionThermostat.Heating;
                    else if (thermostat.Mode == ModeOptionThermostat.Heating)
                        newMode = ModeOptionThermostat.Cooling;
                    else
                        newMode = ModeOptionThermostat.Off;
                    _updateThermostat.Execute(thermostat.DeviceId, thermostat.TargetTemperature, newMode);
                    break;
                case ConsoleKey.T:
                    Console.Write("Target Temp: ");
                    if (double.TryParse(Console.ReadLine(), out double t))
                    {
                        _updateThermostat.Execute(thermostat.DeviceId, new Temperature(t), thermostat.Mode);
                    }
                    break;
                case ConsoleKey.P:
                    Console.Write("Pump #: ");
                    if (int.TryParse(Console.ReadLine(), out int pi) && pi >= 1 && pi <= pumps.Count)
                    {
                        var pump = pumps[pi - 1];
                        if (pump.IsOn) pump.ToggleOff(); else pump.ToggleOn();
                        _updateHeatPump.Execute(pump.DeviceId, pump.TargetTemperature, pump.Power);
                    }
                    break;
                case ConsoleKey.W:
                    Console.Write("Pump #: ");
                    if (int.TryParse(Console.ReadLine(), out int wi) && wi >= 1 && wi <= pumps.Count)
                    {
                        Console.Write("Power (0-100): ");
                        if (int.TryParse(Console.ReadLine(), out int pow))
                        {
                            var pump = pumps[wi - 1];
                            _updateHeatPump.Execute(pump.DeviceId, pump.TargetTemperature, new Power(pow));
                        }
                    }
                    break;
                case ConsoleKey.B: stay = false; break;
            }
        }
    }

    static void FoodMenu()
    {
        bool stay = true;
        var updateCoffee = new UpdateCoffeeMachineCommand(_coffeeRepo);
        var updateFridge = new UpdateRefrigeratorCommand(_fridgeRepo);

        while (stay)
        {
            Console.Clear();
            Console.WriteLine("FOOD & APPLIANCES");
            Console.WriteLine("");
            
            var coffee = _getAllCoffee.Execute().First();
            var fridge = _getAllFridges.Execute().First();

            Console.WriteLine($"Coffee Machine: {coffee.Name} [{(coffee.Status ? "ON" : "OFF")}]");
            Console.WriteLine($"Refrigerator: {fridge.Name}");
            Console.WriteLine($"  Fridge:  {fridge.MyFridge.CurrentTemperature}C {(fridge.MyFridge.IsDoorOpen ? "(OPEN)" : "(CLOSED)")}");
            Console.WriteLine($"  Freezer: {fridge.MyFreezer.CurrentTemperature}C {(fridge.MyFreezer.IsDoorOpen ? "(OPEN)" : "(CLOSED)")}");
            Console.WriteLine("");
            Console.WriteLine("[C] Toggle Coffee Machine");
            Console.WriteLine("[F] Open/Close Fridge");
            Console.WriteLine("[Z] Open/Close Freezer");
            Console.WriteLine("[T] Set Fridge Temp");
            Console.WriteLine("[B] Back");
            Console.Write("> ");

            switch (Console.ReadKey(true).Key)
            {
                case ConsoleKey.C:
                    if (coffee.Status) coffee.ToggleOff(); else coffee.ToggleOn();
                    updateCoffee.Execute(coffee);
                    break;
                case ConsoleKey.F:
                    if (fridge.MyFridge.IsDoorOpen) fridge.CloseFridge(); else fridge.OpenFridge();
                    updateFridge.Execute(fridge);
                    break;
                case ConsoleKey.Z:
                    if (fridge.MyFreezer.IsDoorOpen) fridge.CloseFreezer(); else fridge.OpenFreezer();
                    updateFridge.Execute(fridge);
                    break;
                case ConsoleKey.T:
                    Console.Write("Temp (0-6): ");
                    if (double.TryParse(Console.ReadLine(), out double ft))
                    {
                        fridge.SetMyFridgeTemp(new Temperature(ft));
                        updateFridge.Execute(fridge);
                    }
                    break;
                case ConsoleKey.B: stay = false; break;
            }
        }
    }

    static void ShowFullStatus()
    {
        Console.Clear();
        Console.WriteLine("FULL DEVICE STATUS");
        Console.WriteLine("");
        
        PrintList("Lamps", _getAllLamps.Execute());
        PrintList("Doors", _doorRepo.GetAll());
        Console.WriteLine($"Camera: {_getAllCctv.Execute().First()}");
        Console.WriteLine($"Alarm: {_alarmRepo.GetAll().First()}");
        PrintList("Heat Pumps", _getAllHeatPumps.Execute());
        Console.WriteLine($"Thermostat: {_getAllThermostats.Execute().First()}");
        Console.WriteLine($"Coffee: {_getAllCoffee.Execute().First()}");
        Console.WriteLine($"Refrigerator: {_getAllFridges.Execute().First()}");
        Console.WriteLine("");
        Console.WriteLine("Press any key to back...");
        Console.ReadKey(true);
    }

    static void PrintList<T>(string title, System.Collections.Generic.IEnumerable<T> items)
    {
        Console.WriteLine($"{title}:");
        foreach (var item in items) Console.WriteLine($"  {item}");
    }
}
