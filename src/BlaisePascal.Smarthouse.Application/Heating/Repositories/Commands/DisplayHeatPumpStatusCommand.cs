using System;
using BlaisePascal.SmartHouse.Domain.Heating.Repositories;

namespace BlaisePascal.SmartHouse.Application.Heating.Repositories.Commands
{
    public class DisplayHeatPumpStatusCommand
    {
        private readonly IHeatPumpRepository _heatPumpRepository;

        public DisplayHeatPumpStatusCommand(IHeatPumpRepository heatPumpRepository)
        {
            _heatPumpRepository = heatPumpRepository;
        }

        public string? Execute(Guid id)
        {
            var heatPump = _heatPumpRepository.GetById(id);
            if (heatPump == null) return null;

            return $"Name: {heatPump.Name}\n" +
                   $"Status: {(heatPump.Status ? "ON" : "OFF")}\n" +
                   $"Mode: {heatPump.Mode}\n" +
                   $"Current Temperature: {heatPump.CurrentTemperature}\n" +
                   $"Target Temperature: {heatPump.TargetTemperature}\n" +
                   $"Power: {heatPump.Power}\n" +
                   $"Angle: {heatPump.Angle}\n" +
                   $"Brand: {heatPump.Brand}\n" +
                   $"Model: {heatPump.Model}\n" +
                   $"Energy Class: {heatPump.EnergyEfficiency}";
        }
    }
}
