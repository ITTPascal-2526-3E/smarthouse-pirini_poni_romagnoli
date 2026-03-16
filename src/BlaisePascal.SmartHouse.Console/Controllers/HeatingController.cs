using System;
using System.Collections.Generic;
using System.Linq;

using BlaisePascal.SmartHouse.Domain.ValueObjects;
using BlaisePascal.SmartHouse.Domain.Illumination.LampOptions;
using BlaisePascal.SmartHouse.Domain.Heating.HeatingDevices;
using BlaisePascal.SmartHouse.Domain.Heating.HeatingOptions;

using BlaisePascal.SmartHouse.Infrastructure.Repositories.Devices.Heating;

using BlaisePascal.SmartHouse.Application.Heating.Repositories.Commands;
using BlaisePascal.SmartHouse.Application.Heating.Repositories.Queries;

namespace BlaisePascal.SmartHouse.Console.Controllers
{
    internal sealed class HeatingController
    {
        private readonly InMemoryThermostatRepository _thermostatRepo;
        private readonly InMemoryHeatPumpRepository _heatPumpRepo;

        // Queries
        private readonly GetAllThermostatsQuery _getAllThermostats;
        private readonly GetAllHeatPumpsQuery _getAllHeatPumps;
        private readonly GetHeatPumpByIDQuery _getHeatPumpById;
        private readonly GetHeatPumpActualTemperatureQuery _getHeatPumpActualTemp;
        private readonly GetHeatPumpTargetTemperatureQuery _getHeatPumpTargetTemp;
        private readonly GetThermostatByIDQuery _getThermostatById;
        private readonly GetThermostatActualTemperatureQuery _getThermostatActualTemp;
        private readonly GetThermostatTargetTemperatureQuery _getThermostatTargetTemp;
        private readonly GetThermostatTemperatureQuery _getThermostatTemperature;

        // Commands
        private readonly AddHeatPumpCommand _addHeatPump;
        private readonly AddThermostatCommand _addThermostat;
        private readonly RemoveHeatPumpCommand _removeHeatPump;
        private readonly RemoveThermostatCommand _removeThermostat;
        private readonly UpdateHeatPumpCommand _updateHeatPump;
        private readonly UpdateThermostatCommand _updateThermostat;
        private readonly DisplayHeatPumpStatusCommand _displayHeatPumpStatus;
        private readonly DisplayThermostatStatusCommand _displayThermostatStatus;

        public HeatingController()
        {
            _thermostatRepo = new InMemoryThermostatRepository();
            _heatPumpRepo = new InMemoryHeatPumpRepository();

            _getAllThermostats = new GetAllThermostatsQuery(_thermostatRepo);
            _getAllHeatPumps = new GetAllHeatPumpsQuery(_heatPumpRepo);
            _getHeatPumpById = new GetHeatPumpByIDQuery(_heatPumpRepo);
            _getHeatPumpActualTemp = new GetHeatPumpActualTemperatureQuery(_heatPumpRepo);
            _getHeatPumpTargetTemp = new GetHeatPumpTargetTemperatureQuery(_heatPumpRepo);
            _getThermostatById = new GetThermostatByIDQuery(_thermostatRepo);
            _getThermostatActualTemp = new GetThermostatActualTemperatureQuery(_thermostatRepo);
            _getThermostatTargetTemp = new GetThermostatTargetTemperatureQuery(_thermostatRepo);
            _getThermostatTemperature = new GetThermostatTemperatureQuery(_thermostatRepo);

            _addHeatPump = new AddHeatPumpCommand(_heatPumpRepo);
            _addThermostat = new AddThermostatCommand(_thermostatRepo);
            _removeHeatPump = new RemoveHeatPumpCommand(_heatPumpRepo);
            _removeThermostat = new RemoveThermostatCommand(_thermostatRepo);
            _updateHeatPump = new UpdateHeatPumpCommand(_heatPumpRepo);
            _updateThermostat = new UpdateThermostatCommand(_thermostatRepo);
            _displayHeatPumpStatus = new DisplayHeatPumpStatusCommand(_heatPumpRepo);
            _displayThermostatStatus = new DisplayThermostatStatusCommand(_thermostatRepo);
        }

        public void InitData()
        {
            var hp1 = new HeatPump(new Temperature(20), "Living Room AC");
            var hp2 = new HeatPump(new Temperature(18), "Bedroom AC");
            _heatPumpRepo.Add(hp1);
            _heatPumpRepo.Add(hp2);

            var thermostat = new Thermostat(new Temperature(20), ModeOptionThermostat.Off, new Temperature(22));
            thermostat.AddHeatPump(hp1);
            thermostat.AddHeatPump(hp2);
            _thermostatRepo.Add(thermostat);
        }

        public Thermostat GetThermostat()
        {
            return _getAllThermostats.Execute().First();
        }

        public List<HeatPump> GetAllHeatPumps()
        {
            return _getAllHeatPumps.Execute();
        }

        public void ShowMenu()
        {
            bool stay = true;
            while (stay)
            {
                System.Console.Clear();
                System.Console.WriteLine("HEATING");
                System.Console.WriteLine("");

                var thermostats = _getAllThermostats.Execute();
                var pumps = _getAllHeatPumps.Execute();

                System.Console.WriteLine("Thermostats:");
                for (int i = 0; i < thermostats.Count; i++)
                {
                    var th = thermostats[i];
                    System.Console.WriteLine($"  {i + 1}. Mode: {th.Mode} | Current: {th.CurrentTemperature} | Target: {th.TargetTemperature}");
                }

                System.Console.WriteLine("Heat Pumps:");
                for (int i = 0; i < pumps.Count; i++)
                {
                    var p = pumps[i];
                    string status = p.IsOn ? "ON" : "OFF";
                    System.Console.WriteLine($"  {i + 1}. [{status}] {p.Name} ({p.CurrentTemperature} -> {p.TargetTemperature})");
                }
                System.Console.WriteLine("");
                System.Console.WriteLine("[M] Cycle Thermostat Mode");
                System.Console.WriteLine("[T] Set Thermostat Target Temp");
                System.Console.WriteLine("[P] Toggle Pump");
                System.Console.WriteLine("[W] Set Pump Power");
                System.Console.WriteLine("[N] Add new Heat Pump");
                System.Console.WriteLine("[R] Remove Heat Pump");
                System.Console.WriteLine("[I] Inspect Heat Pump (full status)");
                System.Console.WriteLine("[S] Inspect Thermostat (full status)");
                System.Console.WriteLine("[B] Back");
                System.Console.Write("> ");

                switch (System.Console.ReadKey(true).Key)
                {
                    case ConsoleKey.M:
                        if (thermostats.Count > 0)
                        {
                            var thermostat = thermostats.First();
                            ModeOptionThermostat newMode = thermostat.Mode;
                            if (thermostat.Mode == ModeOptionThermostat.Off)
                                newMode = ModeOptionThermostat.Heating;
                            else if (thermostat.Mode == ModeOptionThermostat.Heating)
                                newMode = ModeOptionThermostat.Cooling;
                            else
                                newMode = ModeOptionThermostat.Off;
                            _updateThermostat.Execute(thermostat.DeviceId, thermostat.TargetTemperature, newMode);
                        }
                        break;
                    case ConsoleKey.T:
                        if (thermostats.Count > 0)
                        {
                            var thermostat = thermostats.First();
                            System.Console.Write("Target Temp (5-40): ");
                            if (double.TryParse(System.Console.ReadLine(), out double t) && t >= 5 && t <= 40)
                            {
                                _updateThermostat.Execute(thermostat.DeviceId, new Temperature(t), thermostat.Mode);
                            }
                            else
                            {
                                System.Console.WriteLine("Invalid temperature. Must be between 5 and 40.");
                                System.Console.Write("Press any key...");
                                System.Console.ReadKey(true);
                            }
                        }
                        break;
                    case ConsoleKey.P:
                        System.Console.Write("Pump #: ");
                        if (int.TryParse(System.Console.ReadLine(), out int pi) && pi >= 1 && pi <= pumps.Count)
                        {
                            var pump = pumps[pi - 1];
                            if (pump.IsOn) pump.ToggleOff(); else pump.ToggleOn();
                            _updateHeatPump.Execute(pump.DeviceId, pump.TargetTemperature, pump.Power);
                        }
                        break;
                    case ConsoleKey.W:
                        System.Console.Write("Pump #: ");
                        if (int.TryParse(System.Console.ReadLine(), out int wi) && wi >= 1 && wi <= pumps.Count)
                        {
                            System.Console.Write("Power (0-100): ");
                            if (int.TryParse(System.Console.ReadLine(), out int pow) && pow >= 0 && pow <= 100)
                            {
                                var pump = pumps[wi - 1];
                                _updateHeatPump.Execute(pump.DeviceId, pump.TargetTemperature, new Power(pow));
                            }
                            else
                            {
                                System.Console.WriteLine("Invalid power. Must be between 0 and 100.");
                                System.Console.Write("Press any key...");
                                System.Console.ReadKey(true);
                            }
                        }
                        break;
                    case ConsoleKey.N:
                        System.Console.Write("Name: ");
                        string? hpName = System.Console.ReadLine();
                        System.Console.Write("Brand: ");
                        string? hpBrand = System.Console.ReadLine();
                        System.Console.Write("Model: ");
                        string? hpModel = System.Console.ReadLine();
                        System.Console.Write("Initial Temp (5-40): ");
                        if (double.TryParse(System.Console.ReadLine(), out double initTemp) && initTemp >= 5 && initTemp <= 40
                            && !string.IsNullOrWhiteSpace(hpName) && !string.IsNullOrWhiteSpace(hpBrand) && !string.IsNullOrWhiteSpace(hpModel))
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
                            _addHeatPump.Execute(new Temperature(initTemp), hpName, hpBrand, hpModel, energy);
                            System.Console.WriteLine("Heat Pump added!");
                            System.Console.Write("Press any key...");
                            System.Console.ReadKey(true);
                        }
                        else
                        {
                            System.Console.WriteLine("Invalid input. Fields cannot be empty, temp must be 5-40.");
                            System.Console.Write("Press any key...");
                            System.Console.ReadKey(true);
                        }
                        break;
                    case ConsoleKey.R:
                        System.Console.Write("Pump # to remove: ");
                        if (int.TryParse(System.Console.ReadLine(), out int ri) && ri >= 1 && ri <= pumps.Count)
                        {
                            _removeHeatPump.Execute(pumps[ri - 1].DeviceId);
                            System.Console.WriteLine("Heat Pump removed!");
                            System.Console.Write("Press any key...");
                            System.Console.ReadKey(true);
                        }
                        break;
                    case ConsoleKey.I:
                        System.Console.Write("Pump #: ");
                        if (int.TryParse(System.Console.ReadLine(), out int ii) && ii >= 1 && ii <= pumps.Count)
                        {
                            var pump = pumps[ii - 1];
                            // DisplayHeatPumpStatusCommand
                            string? statusInfo = _displayHeatPumpStatus.Execute(pump.DeviceId);
                            if (statusInfo != null) System.Console.WriteLine(statusInfo);
                            // GetHeatPumpActualTemperatureQuery
                            var actualTemp = _getHeatPumpActualTemp.Execute(pump.DeviceId);
                            System.Console.WriteLine($"Actual Temp (query): {actualTemp}");
                            // GetHeatPumpTargetTemperatureQuery
                            var targetTemp = _getHeatPumpTargetTemp.Execute(pump.DeviceId);
                            System.Console.WriteLine($"Target Temp (query): {targetTemp}");
                            // GetHeatPumpByIDQuery
                            var pumpById = _getHeatPumpById.Execute(pump.DeviceId);
                            System.Console.WriteLine($"ID: {pumpById?.DeviceId}");
                            System.Console.WriteLine("");
                            System.Console.Write("Press any key...");
                            System.Console.ReadKey(true);
                        }
                        break;
                    case ConsoleKey.S:
                        if (thermostats.Count > 0)
                        {
                            var thermostat = thermostats.First();
                            // DisplayThermostatStatusCommand
                            string? thStatus = _displayThermostatStatus.Execute(thermostat.DeviceId);
                            if (thStatus != null) System.Console.WriteLine(thStatus);
                            // GetThermostatActualTemperatureQuery
                            var thActual = _getThermostatActualTemp.Execute(thermostat.DeviceId);
                            System.Console.WriteLine($"Actual Temp (query): {thActual}");
                            // GetThermostatTargetTemperatureQuery
                            var thTarget = _getThermostatTargetTemp.Execute(thermostat.DeviceId);
                            System.Console.WriteLine($"Target Temp (query): {thTarget}");
                            // GetThermostatTemperatureQuery (returns both)
                            var temps = _getThermostatTemperature.Execute(thermostat.DeviceId);
                            System.Console.WriteLine($"Temps (tuple): Current={temps.Current}, Target={temps.Target}");
                            // GetThermostatByIDQuery
                            var thById = _getThermostatById.Execute(thermostat.DeviceId);
                            System.Console.WriteLine($"ID: {thById?.DeviceId}");
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
