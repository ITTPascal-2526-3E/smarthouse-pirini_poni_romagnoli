using System;

namespace BlaisePascal.SmartHouse.Domain
{
    // Base class for all smart devices in the smart house
    public abstract class Device
    {
        // Unique identifier for the device
        public Guid DeviceId { get; } = Guid.NewGuid();

        // Human-readable name of the device
        public string Name { get; protected set; }

        // Indicates whether the device is currently active or ON
        public bool Status { get; protected set; }

        // Timestamp of device creation in UTC
        public DateTime CreatedAtUtc { get; }

        // Timestamp of last state modification in UTC
        public DateTime LastModifiedAtUtc { get; protected set; }

        // Constructor initializes the device name, status and timestamps
        protected Device(string name, bool status)
        {
            Name = name;
            Status = status;
            CreatedAtUtc = DateTime.UtcNow;
            LastModifiedAtUtc = CreatedAtUtc;
        }

        // Updates the last modified timestamp to the current UTC time
        protected void Touch()
        {
            LastModifiedAtUtc = DateTime.UtcNow;
        }

        // Turns the device ON and updates the last modified timestamp
        public virtual void TurnOn()
        {
            Status = true;
            Touch();
        }

        // Turns the device OFF and updates the last modified timestamp
        public virtual void TurnOff()
        {
            Status = false;
            Touch();
        }

        // Changes the visible device name and updates the last modified timestamp
        public void Rename(string newName)
        {
            Name = newName;
            Touch();
        }
    }
}
