using BlaisePascal.SmartHouse.Domain.Abstraction;
using BlaisePascal.SmartHouse.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using BlaisePascal.SmartHouse.Domain.Illumination.LampTypes;
using BlaisePascal.SmartHouse.Domain.Illumination.LampOptions;
using BlaisePascal.SmartHouse.Domain.Illumination.LampAbstraction;

namespace BlaisePascal.SmartHouse.Domain.Illumination.LampCompositions
{
    // Represents a row of lamps that can be controlled as a group
    public sealed class LampsRow : Device
    {
        // Internal list of lamps in this row
        private readonly List<Lamp> _lamps = new List<Lamp>();

        // Constructor initializes the device with a default name and OFF status
        public LampsRow() : base("Unnamed LampsRow", false)
        {
        }

        // Adds a lamp to the row
        public void AddLamp(Lamp lamp)
        {
            if (lamp == null) return;

            _lamps.Add(lamp);
            Touch();
        }

        // Turns ON all lamps in the row and updates the row status
        public void TurnOnAllLamps()
        {
            Status = true;
            Touch();

            foreach (var lamp in _lamps)
            {
                lamp.ToggleOn();
            }
        }

        // Turns OFF all lamps in the row and updates the row status
        public void TurnOffAllLamps()
        {
            Status = false;
            Touch();

            foreach (var lamp in _lamps)
            {
                lamp.ToggleOff();
            }
        }

        // Sets luminosity for all lamps and updates row status if needed
        public void SetLuminosityAllLamps(Luminosity luminosity)
        {
            // If luminosity is set above zero, consider the row as active
            if (luminosity.Value > 0 && !Status)
            {
                Status = true;
                Touch();
            }

            foreach (var lamp in _lamps)
            {
                lamp.SetLuminosity(luminosity);
            }
        }

        public void SetLuminosityAllLamps(int percentage)
        {
            SetLuminosityAllLamps(new Luminosity(percentage));
        }

        // Turns OFF the lamp at the specified index, if valid
        public void TurnOffLampAtIndex(int index)
        {
            if (index < 0 || index >= _lamps.Count) return;

            _lamps[index].ToggleOff();
            Touch();
        }

        // Turns ON the lamp at the specified index, if valid, and marks the row as active
        public void TurnOnLampAtIndex(int index)
        {
            if (index < 0 || index >= _lamps.Count) return;

            _lamps[index].ToggleOn();
            Status = true;
            Touch();
        }

        // Sets luminosity for the lamp at the specified index, if valid
        public void SetLuminosityAtIndex(int index, Luminosity luminosity)
        {
            if (index < 0 || index >= _lamps.Count) return;

            _lamps[index].SetLuminosity(luminosity);
            Touch();
        }

        public void SetLuminosityAtIndex(int index, int percentage)
        {
            SetLuminosityAtIndex(index, new Luminosity(percentage));
        }

        // Returns the number of lamps that are currently ON in the row
        public int GetOnLampsCount()
        {
            int count = 0;
            foreach (var lamp in _lamps)
            {
                if (lamp.IsOn) count++;
            }
            return count;
        }

        // Returns the total number of lamps in the row
        public int GetLampsCount()
        {
            return _lamps.Count;
        }

        // Returns the lamp at the specified index, or null if the index is invalid
        public Lamp? GetLampAtIndex(int index)
        {
            if (index < 0 || index >= _lamps.Count)
            {
                return null;
            }
            return _lamps[index];
        }

        // Removes the lamp at the specified index, if valid
        public void RemoveLampAtIndex(int index)
        {
            if (index < 0 || index >= _lamps.Count)
            {
                return;
            }
            _lamps.RemoveAt(index);
            Touch();
        }

        // Removes all lamps from the row and marks the row as OFF
        public void ClearAllLamps()
        {
            _lamps.Clear();
            Status = false;
            Touch();
        }

        // Calls Update on all EcoLamp instances contained in the row
        public void UpdateAllEcoLamps(DateTime now)
        {
            foreach (var lamp in _lamps)
            {
                if (lamp is EcoLamp ecoLamp)
                {
                    ecoLamp.Update(now);
                }
            }
        }

        // Calls RegisterPresence on all EcoLamp instances contained in the row
        public void RegisterPresenceAllEcoLamps()
        {
            foreach (var lamp in _lamps)
            {
                if (lamp is EcoLamp ecoLamp)// Check if the lamp is an EcoLamp
                {
                    ecoLamp.RegisterPresence();
                }
            }
        }

        // Schedules ON/OFF times for all EcoLamp instances contained in the row
        public void ScheduleAllEcoLamps(DateTime? onTime, DateTime? offTime)
        {
            foreach (var lamp in _lamps)
            {
                if (lamp is EcoLamp ecoLamp)// Check if the lamp is an EcoLamp
                {
                    ecoLamp.Schedule(onTime, offTime);// Schedule ON/OFF times for all EcoLamp instances contained in the row
                }
            }
        }
    }
}
