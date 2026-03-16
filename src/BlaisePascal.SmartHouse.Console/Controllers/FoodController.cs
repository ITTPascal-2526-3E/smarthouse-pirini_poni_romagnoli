using System;
using System.Linq;

using BlaisePascal.SmartHouse.Domain.ValueObjects;
using BlaisePascal.SmartHouse.Domain.Illumination.LampOptions;
using BlaisePascal.SmartHouse.Domain.Food;

using BlaisePascal.SmartHouse.Infrastructure.Repositories.Devices.Food;

using BlaisePascal.Smarthouse.Application.Food.Repositories.Commands;
using BlaisePascal.Smarthouse.Application.Food.Repositories.Queries;

namespace BlaisePascal.SmartHouse.Console.Controllers
{
    internal sealed class FoodController
    {
        private readonly InMemoryCoffeeMachineRepository _coffeeRepo;
        private readonly InMemoryRefrigeratorRepository _fridgeRepo;

        // Queries
        private readonly GetAllCoffeeMachinesQuery _getAllCoffee;
        private readonly GetAllRefrigeratorsQuery _getAllFridges;
        private readonly GetCoffeeMachineByIDQuery _getCoffeeById;
        private readonly GetRefrigeratorByIDQuery _getFridgeById;

        // Commands
        private readonly AddCoffeeMachineCommand _addCoffee;
        private readonly RemoveCoffeeMachineCommand _removeCoffee;
        private readonly UpdateCoffeeMachineCommand _updateCoffee;
        private readonly AddRefrigeratorCommand _addFridge;
        private readonly RemoveRefrigeratorCommand _removeFridge;
        private readonly UpdateRefrigeratorCommand _updateFridge;

        public FoodController()
        {
            _coffeeRepo = new InMemoryCoffeeMachineRepository();
            _fridgeRepo = new InMemoryRefrigeratorRepository();

            _getAllCoffee = new GetAllCoffeeMachinesQuery(_coffeeRepo);
            _getAllFridges = new GetAllRefrigeratorsQuery(_fridgeRepo);
            _getCoffeeById = new GetCoffeeMachineByIDQuery(_coffeeRepo);
            _getFridgeById = new GetRefrigeratorByIDQuery(_fridgeRepo);

            _addCoffee = new AddCoffeeMachineCommand(_coffeeRepo);
            _removeCoffee = new RemoveCoffeeMachineCommand(_coffeeRepo);
            _updateCoffee = new UpdateCoffeeMachineCommand(_coffeeRepo);
            _addFridge = new AddRefrigeratorCommand(_fridgeRepo);
            _removeFridge = new RemoveRefrigeratorCommand(_fridgeRepo);
            _updateFridge = new UpdateRefrigeratorCommand(_fridgeRepo);
        }

        public void InitData()
        {
            _addCoffee.Execute("Morning Brew", "DeLonghi", "Magnifica", EnergyClass.A, false);

            var fridge = new Fridge("Samsung", "FamilyHub", 500, "Kitchen Fridge");
            var freezer = new Freezer("Samsung", "FamilyHub", 200, "Kitchen Freezer");
            _addFridge.Execute(fridge, "Kitchen Refrigerator", freezer);
        }

        public CoffeeMachine GetCoffeeMachine()
        {
            return _getAllCoffee.Execute().First();
        }

        public Refrigerator GetRefrigerator()
        {
            return _getAllFridges.Execute().First();
        }

        public void ShowMenu()
        {
            bool stay = true;
            while (stay)
            {
                System.Console.Clear();
                System.Console.WriteLine("FOOD & APPLIANCES");
                System.Console.WriteLine("");

                var coffees = _getAllCoffee.Execute();
                var fridges = _getAllFridges.Execute();

                System.Console.WriteLine("Coffee Machines:");
                for (int i = 0; i < coffees.Count; i++)
                {
                    var c = coffees[i];
                    System.Console.WriteLine($"  {i + 1}. {c.Name} [{(c.Status ? "ON" : "OFF")}]");
                }

                System.Console.WriteLine("Refrigerators:");
                for (int i = 0; i < fridges.Count; i++)
                {
                    var f = fridges[i];
                    System.Console.WriteLine($"  {i + 1}. {f.Name}");
                    System.Console.WriteLine($"     Fridge:  {f.MyFridge.CurrentTemperature}C {(f.MyFridge.IsDoorOpen ? "(OPEN)" : "(CLOSED)")}");
                    System.Console.WriteLine($"     Freezer: {f.MyFreezer.CurrentTemperature}C {(f.MyFreezer.IsDoorOpen ? "(OPEN)" : "(CLOSED)")}");
                }
                System.Console.WriteLine("");
                System.Console.WriteLine("[C] Toggle Coffee Machine");
                System.Console.WriteLine("[F] Open/Close Fridge");
                System.Console.WriteLine("[Z] Open/Close Freezer");
                System.Console.WriteLine("[T] Set Fridge Temp");
                System.Console.WriteLine("[N] Add Coffee Machine");
                System.Console.WriteLine("[R] Remove Coffee Machine");
                System.Console.WriteLine("[I] Inspect Coffee Machine (by ID)");
                System.Console.WriteLine("[J] Inspect Refrigerator (by ID)");
                System.Console.WriteLine("[B] Back");
                System.Console.Write("> ");

                switch (System.Console.ReadKey(true).Key)
                {
                    case ConsoleKey.C:
                        System.Console.Write("Coffee Machine #: ");
                        if (int.TryParse(System.Console.ReadLine(), out int ci) && ci >= 1 && ci <= coffees.Count)
                        {
                            var coffee = coffees[ci - 1];
                            if (coffee.Status) coffee.ToggleOff(); else coffee.ToggleOn();
                            _updateCoffee.Execute(coffee);
                        }
                        break;
                    case ConsoleKey.F:
                        System.Console.Write("Refrigerator #: ");
                        if (int.TryParse(System.Console.ReadLine(), out int fi) && fi >= 1 && fi <= fridges.Count)
                        {
                            var fridge = fridges[fi - 1];
                            if (fridge.MyFridge.IsDoorOpen) fridge.CloseFridge(); else fridge.OpenFridge();
                            _updateFridge.Execute(fridge);
                        }
                        break;
                    case ConsoleKey.Z:
                        System.Console.Write("Refrigerator #: ");
                        if (int.TryParse(System.Console.ReadLine(), out int zi) && zi >= 1 && zi <= fridges.Count)
                        {
                            var fridge = fridges[zi - 1];
                            if (fridge.MyFreezer.IsDoorOpen) fridge.CloseFreezer(); else fridge.OpenFreezer();
                            _updateFridge.Execute(fridge);
                        }
                        break;
                    case ConsoleKey.T:
                        System.Console.Write("Refrigerator #: ");
                        if (int.TryParse(System.Console.ReadLine(), out int ti) && ti >= 1 && ti <= fridges.Count)
                        {
                            System.Console.Write("Temp (0-6): ");
                            if (double.TryParse(System.Console.ReadLine(), out double ft) && ft >= 0 && ft <= 6)
                            {
                                var fridge = fridges[ti - 1];
                                fridge.SetMyFridgeTemp(new Temperature(ft));
                                _updateFridge.Execute(fridge);
                            }
                            else
                            {
                                System.Console.WriteLine("Invalid temperature. Must be between 0 and 6.");
                                System.Console.Write("Press any key...");
                                System.Console.ReadKey(true);
                            }
                        }
                        break;
                    case ConsoleKey.N:
                        System.Console.Write("Name: ");
                        string? cmName = System.Console.ReadLine();
                        System.Console.Write("Brand: ");
                        string? cmBrand = System.Console.ReadLine();
                        System.Console.Write("Model: ");
                        string? cmModel = System.Console.ReadLine();
                        if (!string.IsNullOrWhiteSpace(cmName) && !string.IsNullOrWhiteSpace(cmBrand) && !string.IsNullOrWhiteSpace(cmModel))
                        {
                            System.Console.WriteLine("Energy Class: 1=A++ 2=A+ 3=A 4=B 5=C");
                            System.Console.Write("> ");
                            EnergyClass energy = EnergyClass.A;
                            if (int.TryParse(System.Console.ReadLine(), out int ec))
                            {
                                if (ec == 1) energy = EnergyClass.APlusPlus;
                                else if (ec == 2) energy = EnergyClass.APlus;
                                else if (ec == 3) energy = EnergyClass.A;
                                else if (ec == 4) energy = EnergyClass.B;
                                else if (ec == 5) energy = EnergyClass.C;
                            }
                            _addCoffee.Execute(cmName, cmBrand, cmModel, energy, false);
                            System.Console.WriteLine("Coffee Machine added!");
                            System.Console.Write("Press any key...");
                            System.Console.ReadKey(true);
                        }
                        else
                        {
                            System.Console.WriteLine("Invalid input. Name, Brand, and Model cannot be empty.");
                            System.Console.Write("Press any key...");
                            System.Console.ReadKey(true);
                        }
                        break;
                    case ConsoleKey.R:
                        System.Console.Write("Coffee Machine # to remove: ");
                        if (int.TryParse(System.Console.ReadLine(), out int ri) && ri >= 1 && ri <= coffees.Count)
                        {
                            _removeCoffee.Execute(coffees[ri - 1].DeviceId);
                            System.Console.WriteLine("Coffee Machine removed!");
                            System.Console.Write("Press any key...");
                            System.Console.ReadKey(true);
                        }
                        break;
                    case ConsoleKey.I:
                        System.Console.Write("Coffee Machine #: ");
                        if (int.TryParse(System.Console.ReadLine(), out int ii) && ii >= 1 && ii <= coffees.Count)
                        {
                            var coffee = coffees[ii - 1];
                            // GetCoffeeMachineByIDQuery
                            var cmById = _getCoffeeById.Execute(coffee.DeviceId);
                            if (cmById != null)
                            {
                                System.Console.WriteLine($"Name: {cmById.Name}");
                                System.Console.WriteLine($"Brand: {cmById.Brand}");
                                System.Console.WriteLine($"Model: {cmById.Model}");
                                System.Console.WriteLine($"Status: {(cmById.Status ? "ON" : "OFF")}");
                                System.Console.WriteLine($"ID: {cmById.DeviceId}");
                            }
                            else
                            {
                                System.Console.WriteLine("Coffee Machine not found.");
                            }
                            System.Console.WriteLine("");
                            System.Console.Write("Press any key...");
                            System.Console.ReadKey(true);
                        }
                        break;
                    case ConsoleKey.J:
                        System.Console.Write("Refrigerator #: ");
                        if (int.TryParse(System.Console.ReadLine(), out int ji) && ji >= 1 && ji <= fridges.Count)
                        {
                            var fridge = fridges[ji - 1];
                            // GetRefrigeratorByIDQuery
                            var frById = _getFridgeById.Execute(fridge.DeviceId);
                            if (frById != null)
                            {
                                System.Console.WriteLine($"Name: {frById.Name}");
                                System.Console.WriteLine($"Fridge Temp: {frById.MyFridge.CurrentTemperature}C");
                                System.Console.WriteLine($"Freezer Temp: {frById.MyFreezer.CurrentTemperature}C");
                                System.Console.WriteLine($"ID: {frById.DeviceId}");
                            }
                            else
                            {
                                System.Console.WriteLine("Refrigerator not found.");
                            }
                            System.Console.WriteLine("");
                            System.Console.Write("Press any key...");
                            System.Console.ReadKey(true);
                        }
                        break;
                    case ConsoleKey.B: stay = false; break;
                }
            }
        }
    }
}
