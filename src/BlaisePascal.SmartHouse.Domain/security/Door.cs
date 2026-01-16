using System;
using BlaisePascal.SmartHouse.Domain.Abstraction;

namespace BlaisePascal.SmartHouse.Domain.security
{
    // Represents a smart door that can be opened, closed, locked and unlocked
    public class Door : SecurityDevice
    {
        // Indicates whether the door is locked
        public bool IsLocked { get; private set; }

        // Constructor initializes the door with a name, status and initial locked state
        public Door(string name, bool status)
            : base(name, status)
        {
            // By default the door starts locked
            IsLocked = true;
            Touch();
        }

        // Opens the door only if it is not locked and updates the device status
        public void OpenDoor()
        {
            if (!IsLocked)
            {
                // Status = true means the door is open
                ToggleOn();
                Touch();
            }
        }

        // Closes the door and updates the device status
        public void CloseDoor()
        {
            // Status = false means the door is closed
            ToggleOff();
            Touch();
        }

        // Locks the door and prevents it from being opened without a key
        public void LockDoor()
        {
            IsLocked = true;
            Touch();
        }

        // Unlocks the door and allows it to be opened
        public void UnlockDoor()
        {
            IsLocked = false;
            Touch();
        }

        // Opens the door using a key: unlocks the door and sets the status to open
        public void OpenDoorWithKey()
        {
            // Unlock the door first
            IsLocked = false;
            // Then open the door
            ToggleOn();
            Touch();
        }

        // Closes and locks the door using a key
        public void CloseDoorWithKey()
        {
            // Close the door
            ToggleOff();
            // Lock the door after closing
            IsLocked = true;
            Touch();
        }
        public override void TriggerAlarm()
        {
            // Implementation specific to Door (e.g. log intrusion)
            Console.WriteLine($"ALARM: Door '{Name}' breached!");
        }

        public override string ToString()
        {
            return $"{base.ToString()}, Locked: {IsLocked}";
        }
    }
}
