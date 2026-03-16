using System;
using System.Collections.Generic;

using BlaisePascal.SmartHouse.Domain.ValueObjects;
using BlaisePascal.SmartHouse.Domain.Illumination.LampOptions;
using BlaisePascal.SmartHouse.Domain.Illumination.LampTypes;

using BlaisePascal.SmartHouse.Infrastructure.Repositories.Devices.Illumination.Lamps;

using BlaisePascal.SmartHouse.Application.Illumination.Repositories.Commands;
using BlaisePascal.SmartHouse.Application.Illumination.Repositories.Queries;

namespace BlaisePascal.SmartHouse.Console.Controllers
{
    internal sealed class LampController
    {
        private readonly InMemoryLampRepository _lampRepo;

        // Queries
        private readonly GetAllLampsQuery _getAllLamps;
        private readonly GetLampByIDQuery _getLampById;
        private readonly GetLampCurrentBrightnessQuery _getLampBrightness;

        // Commands
        private readonly AddLampCommand _addLamp;
        private readonly RemoveLampCommand _removeLamp;
        private readonly SwitchOnLampCommand _switchOnLamp;
        private readonly SwitchOffLampCommand _switchOffLamp;
        private readonly UpdateLampCommand _updateLamp;
        private readonly DisplayLampStatusCommand _displayLampStatus;

        public LampController()
        {
            _lampRepo = new InMemoryLampRepository();

            _getAllLamps = new GetAllLampsQuery(_lampRepo);
            _getLampById = new GetLampByIDQuery(_lampRepo);
            _getLampBrightness = new GetLampCurrentBrightnessQuery(_lampRepo);

            _addLamp = new AddLampCommand(_lampRepo);
            _removeLamp = new RemoveLampCommand(_lampRepo);
            _switchOnLamp = new SwitchOnLampCommand(_lampRepo);
            _switchOffLamp = new SwitchOffLampCommand(_lampRepo);
            _updateLamp = new UpdateLampCommand(_lampRepo);
            _displayLampStatus = new DisplayLampStatusCommand(_lampRepo);
        }

        public void InitData()
        {
            _addLamp.Execute(60, ColorOption.WarmWhite, "Philips", "Living Room Lamp", EnergyClass.A, "L-Living");
            _addLamp.Execute(40, ColorOption.NeutralWhite, "Osram", "Hallway EcoLamp", EnergyClass.A, "Eco-Hall");
            _addLamp.Execute(1, ColorOption.CoolWhite, "Samsung", "LED Strip", EnergyClass.A, "Strip-1");
        }

        public List<Lamp> GetAll()
        {
            return _getAllLamps.Execute();
        }

        public void ShowMenu()
        {
            bool stay = true;
            while (stay)
            {
                System.Console.Clear();
                System.Console.WriteLine("ILLUMINATION");
                System.Console.WriteLine("");
                var lamps = _getAllLamps.Execute();

                for (int i = 0; i < lamps.Count; i++)
                {
                    var l = lamps[i];
                    string status = l.IsOn ? "ON" : "OFF";
                    System.Console.WriteLine($"{i + 1}. [{status}] {l.Name} (Lum: {l.CurrentLuminosity?.Value ?? 0})");
                }
                System.Console.WriteLine("");
                System.Console.WriteLine("[T] Toggle lamp #");
                System.Console.WriteLine("[A] Turn All ON");
                System.Console.WriteLine("[O] Turn All OFF");
                System.Console.WriteLine("[L] Set luminosity");
                System.Console.WriteLine("[N] Add new lamp");
                System.Console.WriteLine("[R] Remove lamp");
                System.Console.WriteLine("[I] Inspect lamp (status + brightness)");
                System.Console.WriteLine("[B] Back");
                System.Console.Write("> ");

                switch (System.Console.ReadKey(true).Key)
                {
                    case ConsoleKey.T:
                        System.Console.Write("Lamp #: ");
                        if (int.TryParse(System.Console.ReadLine(), out int ti) && ti >= 1 && ti <= lamps.Count)
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
                        System.Console.Write("Lamp #: ");
                        if (int.TryParse(System.Console.ReadLine(), out int li) && li >= 1 && li <= lamps.Count)
                        {
                            System.Console.Write("Luminosity (0-100): ");
                            if (int.TryParse(System.Console.ReadLine(), out int lum) && lum >= 0 && lum <= 100)
                            {
                                var lamp = lamps[li - 1];
                                if (!lamp.IsOn) _switchOnLamp.Execute(lamp.DeviceId);

                                lamp.SetLuminosity(lum);
                                _updateLamp.Execute(lamp.DeviceId);
                            }
                            else
                            {
                                System.Console.WriteLine("Invalid luminosity. Must be between 0 and 100.");
                                System.Console.Write("Press any key...");
                                System.Console.ReadKey(true);
                            }
                        }
                        break;
                    case ConsoleKey.N:
                        System.Console.Write("Name: ");
                        string? name = System.Console.ReadLine();
                        System.Console.Write("Brand: ");
                        string? brand = System.Console.ReadLine();
                        System.Console.Write("Power (watts, > 0): ");
                        if (int.TryParse(System.Console.ReadLine(), out int power) && power > 0
                            && !string.IsNullOrWhiteSpace(name) && !string.IsNullOrWhiteSpace(brand))
                        {
                            System.Console.WriteLine("Color: 1=WarmWhite 2=NeutralWhite 3=CoolWhite");
                            System.Console.Write("> ");
                            if (!int.TryParse(System.Console.ReadLine(), out int colorChoice) || colorChoice < 1 || colorChoice > 3)
                            {
                                System.Console.WriteLine("Invalid color choice. Must be 1, 2, or 3.");
                                System.Console.Write("Press any key...");
                                System.Console.ReadKey(true);
                                break;
                            }
                            ColorOption color = colorChoice == 1 ? ColorOption.WarmWhite : (colorChoice == 2 ? ColorOption.NeutralWhite : ColorOption.CoolWhite);

                            System.Console.WriteLine("Energy Class: 1=A++ 2=A+ 3=A 4=B 5=C");
                            System.Console.Write("> ");
                            if (!int.TryParse(System.Console.ReadLine(), out int energyChoice) || energyChoice < 1 || energyChoice > 5)
                            {
                                System.Console.WriteLine("Invalid energy class choice. Must be between 1 and 5.");
                                System.Console.Write("Press any key...");
                                System.Console.ReadKey(true);
                                break;
                            }
                            EnergyClass energy = EnergyClass.A;
                            if (energyChoice == 1) energy = EnergyClass.APlusPlus;
                            else if (energyChoice == 2) energy = EnergyClass.APlus;
                            else if (energyChoice == 4) energy = EnergyClass.B;
                            else if (energyChoice == 5) energy = EnergyClass.C;
                            _addLamp.Execute(power, color, brand, name, energy, name);
                            System.Console.WriteLine("Lamp added!");
                            System.Console.Write("Press any key...");
                            System.Console.ReadKey(true);
                        }
                        else
                        {
                            System.Console.WriteLine("Invalid input. Name and Brand cannot be empty, Power must be > 0.");
                            System.Console.Write("Press any key...");
                            System.Console.ReadKey(true);
                        }
                        break;
                    case ConsoleKey.R:
                        System.Console.Write("Lamp # to remove: ");
                        if (int.TryParse(System.Console.ReadLine(), out int ri) && ri >= 1 && ri <= lamps.Count)
                        {
                            _removeLamp.Execute(lamps[ri - 1].DeviceId);
                            System.Console.WriteLine("Lamp removed!");
                            System.Console.Write("Press any key...");
                            System.Console.ReadKey(true);
                        }
                        break;
                    case ConsoleKey.I:
                        System.Console.Write("Lamp #: ");
                        if (int.TryParse(System.Console.ReadLine(), out int ii) && ii >= 1 && ii <= lamps.Count)
                        {
                            var lamp = lamps[ii - 1];
                            // DisplayLampStatusCommand
                            string? statusInfo = _displayLampStatus.Execute(lamp.DeviceId);
                            if (statusInfo != null)
                            {
                                System.Console.WriteLine(statusInfo);
                            }
                            // GetLampCurrentBrightnessQuery
                            var brightness = _getLampBrightness.Execute(lamp.DeviceId);
                            System.Console.WriteLine($"Current Brightness: {brightness?.Value ?? 0}");
                            // GetLampByIDQuery
                            var lampById = _getLampById.Execute(lamp.DeviceId);
                            System.Console.WriteLine($"ID: {lampById?.DeviceId}");
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
