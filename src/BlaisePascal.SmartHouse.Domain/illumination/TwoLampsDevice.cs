using BlaisePascal.SmartHouse.Domain.Abstraction;
using System;

namespace BlaisePascal.SmartHouse.Domain
{
    // Represents a smart device composed of up to two lamps
    public sealed class TwoLampsDevice : Device
    {
        // First lamp slot (can be a Lamp or EcoLamp)
        public Lamp? LampA { get; private set; }

        // Second lamp slot (can be a Lamp or EcoLamp)
        public Lamp? LampB { get; private set; }

        // Constructor initializes the device with a default name and OFF status
        public TwoLampsDevice()
            : base("Unnamed TwoLampsDevice", false)
        {
        }

        // Constructor initializes the device with an explicit name and OFF status
        public TwoLampsDevice(string name)
            : base(name, false)
        {
        }

        // Adds a lamp to the first free slot (LampA, then LampB). If both are used, nothing happens
        public void AddLamp(Lamp lamp)
        {
            if (lamp == null)
            {
                return;
            }

            if (LampA == null)
            {
                LampA = lamp;
            }
            else if (LampB == null)
            {
                LampB = lamp;
            }
            else
            {
                // Both slots are already taken
                return;
            }

            Touch();
        }

        // Returns the number of lamps currently assigned (0, 1, or 2)
        public int GetLampsCount()
        {
            int count = 0;
            if (LampA != null) count++;
            if (LampB != null) count++;
            return count;
        }

        // Returns the number of lamps currently ON (0–2)
        public int GetOnLampsCount()
        {
            int count = 0;

            if (LampA != null && LampA.IsOn)
            {
                count++;
            }

            if (LampB != null && LampB.IsOn)
            {
                count++;
            }

            return count;
        }

        // Returns the lamp at the given index (0 -> LampA, 1 -> LampB), or null if index invalid or empty
        public Lamp? GetLampAtIndex(int index)
        {
            return index switch
            {
                0 => LampA,
                1 => LampB,
                _ => null
            };
        }

        // Removes the lamp at the given index and updates device status if no lamps remain
        public void RemoveLampAtIndex(int index)
        {
            switch (index)
            {
                case 0:
                    LampA = null;
                    break;
                case 1:
                    LampB = null;
                    break;
                default:
                    return;
            }

            if (LampA == null && LampB == null)
            {
                Status = false;
            }

            Touch();
        }

        // Removes both lamps and turns the device OFF
        public void ClearAllLamps()
        {
            LampA = null;
            LampB = null;
            Status = false;
            Touch();
        }

        // Turns ON both lamps (if present) and marks the device as active
        public void TurnOnBothLamps()
        {
            if (LampA == null && LampB == null)
            {
                return;
            }

            if (LampA != null)
            {
                LampA.ToggleOn();
            }

            if (LampB != null)
            {
                LampB.ToggleOn();
            }

            Status = true;
            Touch();
        }

        // Turns OFF both lamps (if present) and marks the device as OFF
        public void TurnOffBothLamps()
        {
            if (LampA == null && LampB == null)
            {
                return;
            }

            if (LampA != null)
            {
                LampA.ToggleOff();
            }

            if (LampB != null)
            {
                LampB.ToggleOff();
            }

            Status = false;
            Touch();
        }

        // Sets luminosity for both lamps and updates device status if luminosity is greater than zero
        public void SetLuminosityBothLamps(int percentage)
        {
            if (LampA == null && LampB == null)
            {
                return;
            }

            if (percentage > 0 && !Status)
            {
                ToggleOn();
                Touch();
            }
            else
            {
                Touch();
            }

            if (LampA != null)
            {
                LampA.SetLuminosity(percentage);
            }

            if (LampB != null)
            {
                LampB.SetLuminosity(percentage);
            }
        }

        // Turns OFF the lamp at the given index (0 or 1), if it exists
        public void TurnOffLampAtIndex(int index)
        {
            var lamp = GetLampAtIndex(index);
            if (lamp == null)
            {
                return;
            }

            lamp.ToggleOff();

            // If both lamps are now OFF (or null), set device status to OFF
            if (GetOnLampsCount() == 0)
            {
                ToggleOff();
            }

            Touch();
        }

        // Turns ON the lamp at the given index (0 or 1), if it exists, and marks the device as active
        public void TurnOnLampAtIndex(int index)
        {
            var lamp = GetLampAtIndex(index);
            if (lamp == null)
            {
                return;
            }

            lamp.ToggleOn();
            Status = true;
            Touch();
        }

        // Sets luminosity for the lamp at the given index (0 or 1), if it exists
        public void SetLuminosityAtIndex(int index, int percentage)
        {
            var lamp = GetLampAtIndex(index);
            if (lamp == null)
            {
                return;
            }

            lamp.SetLuminosity(percentage);

            if (percentage > 0 && !Status)
            {
                Status = true;
            }

            Touch();
        }

        // Calls Update on both EcoLamp instances contained in this device
        public void UpdateBothEcoLamps(DateTime now)
        {
            if (LampA is EcoLamp ecoA)
            {
                ecoA.Update(now);
            }

            if (LampB is EcoLamp ecoB)
            {
                ecoB.Update(now);
            }
        }

        // Calls RegisterPresence on both EcoLamp instances contained in this device
        public void RegisterPresenceBothEcoLamps()
        {
            if (LampA is EcoLamp ecoA)
            {
                ecoA.RegisterPresence();
            }

            if (LampB is EcoLamp ecoB)
            {
                ecoB.RegisterPresence();
            }
        }

        // Schedules ON/OFF times for both EcoLamp instances contained in this device
        public void ScheduleBothEcoLamps(DateTime? onTime, DateTime? offTime)
        {
            if (LampA is EcoLamp ecoA)
            {
                ecoA.Schedule(onTime, offTime);
            }

            if (LampB is EcoLamp ecoB)
            {
                ecoB.Schedule(onTime, offTime);
            }
        }
    }
}
