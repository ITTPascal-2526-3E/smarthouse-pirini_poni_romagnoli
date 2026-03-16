using System;
using System.Linq;

using BlaisePascal.SmartHouse.Console.Controllers;

internal sealed class Program
{
    static LampController _lampController = new LampController();
    static SecurityController _securityController = new SecurityController();
    static HeatingController _heatingController = new HeatingController();
    static FoodController _foodController = new FoodController();

    static void Main(string[] args)
    {
        InitData();

        bool running = true;
        while (running)
        {
            Console.Clear();
            Console.WriteLine("SMART HOUSE CONTROL (USE CASES OVER CQRS)");
            Console.WriteLine("");

            var lamps = _lampController.GetAll();
            var alarm = _securityController.GetAlarm();
            var thermostat = _heatingController.GetThermostat();

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
                case ConsoleKey.D1: _lampController.ShowMenu(); break;
                case ConsoleKey.D2: _securityController.ShowMenu(); break;
                case ConsoleKey.D3: _heatingController.ShowMenu(); break;
                case ConsoleKey.D4: _foodController.ShowMenu(); break;
                case ConsoleKey.D5: ShowFullStatus(); break;
                case ConsoleKey.Q: running = false; break;
            }
        }
    }

    static void InitData()
    {
        _lampController.InitData();
        _securityController.InitData();
        _heatingController.InitData();
        _foodController.InitData();
    }

    static void ShowFullStatus()
    {
        Console.Clear();
        Console.WriteLine("FULL DEVICE STATUS");
        Console.WriteLine("");

        PrintList("Lamps", _lampController.GetAll());
        PrintList("Doors", _securityController.GetAllDoors());
        Console.WriteLine($"Camera: {_securityController.GetCamera()}");
        Console.WriteLine($"Alarm: {_securityController.GetAlarm()}");
        PrintList("Heat Pumps", _heatingController.GetAllHeatPumps());
        Console.WriteLine($"Thermostat: {_heatingController.GetThermostat()}");
        Console.WriteLine($"Coffee: {_foodController.GetCoffeeMachine()}");
        Console.WriteLine($"Refrigerator: {_foodController.GetRefrigerator()}");
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
