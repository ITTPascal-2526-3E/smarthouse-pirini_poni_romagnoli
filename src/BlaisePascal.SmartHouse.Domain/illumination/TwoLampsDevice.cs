using System;
using System.Collections.Generic;

namespace BlaisePascal.SmartHouse.Domain
{
    // Represents a smart device composed of exactly up to two lamps
    public class TwoLampsDevice : Device
    {
        // Maximum number of lamps supported by this device
        public const int MaxLamps = 2;

        // Internal list of lamps contained in this device
        private readonly List<Lamp> _lamps = new List<Lamp>(MaxLamps);

        // Constructor initializes the device with a default name and OFF status
        public TwoLampsDevice() : base("Unnamed TwoLampsDevice", false)
        {
        }

        // Constructor initializes the device with an explicit name and OFF status
        public TwoLampsDevice(string name) : base(name, false)
        {
        }

        // Adds a lamp to the device if there is free space (up to two lamps)
        public void AddLamp(Lamp lamp)
        {
            if (lamp == null) return;

            if (_lamps.Count >= MaxLamps)
            {
                // No more than two lamps are allowed in this device
                return;
            }

            _lamps.Add(lamp);
            Touch();
        }

        // Returns the total number of lamps currently contained in the device
        public int GetLampsCount()
        {
            return _lamps.Count;
        }

        // Returns the number of lamps that are currently ON
        public int GetOnLampsCount()
        {
            int count = 0;
            foreach (var lamp in _lamps)
            {
                if (lamp.IsOn) count++;
            }
            return count;
        }

        // Returns the lamp at the given index (0 or 1), or null if the index is invalid
        public Lamp? GetLampAtIndex(int index)
        {
            if (index < 0 || index >= _lamps.Count)
            {
                return null;
            }
            return _lamps[index];
        }

        // Removes the lamp at the given index (0 or 1), if it exists
        public void RemoveLampAtIndex(int index)
        {
            if (index < 0 || index >= _lamps.Count)
            {
                return;
            }

            _lamps.RemoveAt(index);
            // If no lamps remain, we consider the device OFF
            if (_lamps.Count == 0)
            {
                Status = false;
            }

            Touch();
        }

        // Removes all lamps from this device and sets its status to OFF
        public void ClearAllLamps()
        {
            _lamps.Clear();
            Status = false;
            Touch();
        }

        // Turns ON both lamps contained in this device (if present) and marks the device as active
        public void TurnOnBothLamps()
        {
            if (_lamps.Count == 0) return;

            Status = true;
            Touch();

            foreach (var lamp in _lamps)
            {
                lamp.TurnOn();
            }
        }

        // Turns OFF both lamps contained in this device (if present) and marks the device as OFF
        public void TurnOffBothLamps()
        {
            if (_lamps.Count == 0) return;

            Status = false;
            Touch();

            foreach (var lamp in _lamps)
            {
                lamp.TurnOff();
            }
        }

        // Sets luminosity for both lamps and updates device status depending on the requested value
        public void SetLuminosityBothLamps(int percentage)
        {
            if (_lamps.Count == 0) return;

            // If luminosity is set above zero, consider the device as active
            if (percentage > 0 && !Status)
            {
                Status = true;
                Touch();
            }

            foreach (var lamp in _lamps)
            {
                lamp.SetLuminosity(percentage);
            }
        }

        // Turns OFF the lamp at the given index (0 or 1), if it exists
        public void TurnOffLampAtIndex(int index)
        {
            if (index < 0 || index >= _lamps.Count) return;

            _lamps[index].TurnOff();
            Touch();
        }

        // Turns ON the lamp at the given index (0 or 1), if it exists, and marks the device as active
        public void TurnOnLampAtIndex(int index)
        {
            if (index < 0 || index >= _lamps.Count) return;

            _lamps[index].TurnOn();
            Status = true;
            Touch();
        }

        // Sets luminosity for the lamp at the given index (0 or 1), if it exists
        public void SetLuminosityAtIndex(int index, int percentage)
        {
            if (index < 0 || index >= _lamps.Count) return;

            _lamps[index].SetLuminosity(percentage);
            Touch();
        }

        // Calls Update on both EcoLamp instances contained in this device
        public void UpdateBothEcoLamps(DateTime now)
        {
            foreach (var lamp in _lamps)
            {
                if (lamp is EcoLamp ecoLamp)
                {
                    ecoLamp.Update(now);
                }
            }
        }

        // Calls RegisterPresence on both EcoLamp instances contained in this device
        public void RegisterPresenceBothEcoLamps()
        {
            foreach (var lamp in _lamps)
            {
                if (lamp is EcoLamp ecoLamp)
                {
                    ecoLamp.RegisterPresence();
                }
            }
        }

        // Schedules ON/OFF times for both EcoLamp instances contained in this device
        public void ScheduleBothEcoLamps(DateTime? onTime, DateTime? offTime)
        {
            foreach (var lamp in _lamps)
            {
                if (lamp is EcoLamp ecoLamp)
                {
                    ecoLamp.Schedule(onTime, offTime);
                }
            }
        }
    }
}
