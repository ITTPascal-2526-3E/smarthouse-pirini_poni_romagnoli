using BlaisePascal.SmartHouse.Domain.Illumination.LampOptions;
using BlaisePascal.SmartHouse.Domain.Illumination.LampTypes;
using BlaisePascal.SmartHouse.Domain.Illumination.Repositories;
using BlaisePascal.SmartHouse.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BlaisePascal.SmartHouse.Infrastructure.Repositories.Devices.Illumination.Lamps
{
    internal class CsvLampRepository : ILampRepository
    {
        private readonly string _csvFilePath = "lamps.csv";
        public CsvLampRepository()
        {
            var solutionRoot = LocalPathHelper.GetSolutionRoot();

            var dataFoleder = Path.Combine(solutionRoot, "data");
            Directory.CreateDirectory(dataFoleder);

            _csvFilePath = Path.Combine(dataFoleder, "lamps.csv");

            if (!File.Exists(_csvFilePath))
            {
                Save(new List<Lamp>());
            }

        }

        public void Add(Lamp lamp)
        {
            var lamps = Load();
            lamps.Add(lamp);
            Save(lamps);
        }

        public void Update(Lamp lamp)
        {
            var lamps = Load();
            var index = lamps.FindIndex(l => l.DeviceId == lamp.DeviceId);
            if (index != -1)
            {
                lamps[index] = lamp;
                Save(lamps);
            }
        }

        public void Remove(Guid id)
        {
            var lamps = Load();
            var index = lamps.FindIndex(l => l.DeviceId == id);
            if (index != -1)
            {
                lamps.RemoveAt(index);
                Save(lamps);
            }
        }

        public Lamp? GetById(Guid id)
        {
            var lamps = Load();
            return lamps.FirstOrDefault(l => l.DeviceId == id);
        }

        




        public List<Lamp> GetAll()
        {
            return Load();
        }


        private void Save(List<Lamp> lamps)
        {
            var dtos = lamps;
            var lines = new List<string>
            {
                "Id,Name,Status,Power,Color,Model,Brand,EnergyEfficiency,Luminosity,LastModified"
            };

            foreach (var dto in dtos)
            {
                lines.Add(string.Join(",",
                    dto.DeviceId,
                    dto.Name,
                    dto.Status,
                    dto.Power,
                    dto.Color,
                    dto.Model,
                    dto.Brand,
                    dto.EnergyEfficiency,
                    dto.Luminosity.Value,
                    dto.LastModifiedAtUtc.ToString("o")
                ));
            }

            File.WriteAllLines(_csvFilePath, lines);
        }

        private ColorOption ParseColor(string color)
        {
            return color switch
            {
                "White" => ColorOption.NeutralWhite,
                "WarmWhite" => ColorOption.WarmWhite,
                "CoolWhite" => ColorOption.CoolWhite,
                "Red" => ColorOption.Red,
                "Green" => ColorOption.Green,
                "Blue" => ColorOption.Blue,
                _ => throw new ArgumentException($"Invalid color option: {color}")
            };
        }


        private EnergyClass ParseEnergyClass(string energyClass)
        {
            return energyClass switch
            {
                "A++" => EnergyClass.APlusPlus,
                "A+" => EnergyClass.APlus,
                "A" => EnergyClass.A,
                "B" => EnergyClass.B,
                "C" => EnergyClass.C,
                "D" => EnergyClass.D,
                _ => throw new ArgumentException($"Invalid energy class: {energyClass}")
            };
        }


        private List<Lamp> Load()
        {
            var lamps = new List<Lamp>();

            foreach (var line in File.ReadLines(_csvFilePath).Skip(1))
            {
                var parts = line.Split(',');

                var power = int.Parse(parts[3]);
                var color = ParseColor(parts[4]);
                var model = parts[5];
                var brand = parts[6];
                var energyClass = ParseEnergyClass(parts[7]);
                var name = parts[1];

                var lamp = new Lamp(power, color, model, brand, energyClass, name);
                lamp.DeviceId = Guid.Parse(parts[0]);
                lamp.Status = bool.Parse(parts[2]);
                lamp.CurrentLuminosity = new Luminosity(int.Parse(parts[8]));
                lamp.LastModifiedAtUtc = DateTime.Parse(parts[9]);

                lamps.Add(lamp);
            }

            return lamps;
        }
    }
}

