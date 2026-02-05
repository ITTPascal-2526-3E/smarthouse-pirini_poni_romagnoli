using System;
using BlaisePascal.SmartHouse.Domain.ValueObjects;

namespace BlaisePascal.SmartHouse.Domain.Abstraction
{
    // Base class for all smart devices in the smart house
    public abstract class Device : IToggable
    {
        // Unique identifier for the device
        public Guid DeviceId { get; } = Guid.NewGuid();

        // Human-readable name of the device
        public DeviceName Name { get; protected set; }// protected means that only this class and derived can modify it

        // Indicates whether the device is currently ON or OFF
        public bool Status { get; protected set; }

        // Timestamp of device creation in UTC
        public DateTime CreatedAtUtc { get; }

        // Timestamp of last state modification in UTC
        public DateTime LastModifiedAtUtc { get; protected set; }

        // Constructor initializes the device name, status and timestamps
        protected Device(string name, bool status)
        {
            Name = new DeviceName(name);
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
        public virtual void ToggleOn()
        {
            Status = true;
            Touch();
        }

        // Turns the device OFF and updates the last modified timestamp
        public virtual void ToggleOff()
        {
            Status = false;
            Touch();
        }

        // Changes the visible device name and updates the last modified timestamp
        public void Rename(string newName)
        {
            Name = new DeviceName(newName);
            Touch();
        }

        // Returns a string representation of the device, Status can be null beacause not every device has necessarly one
        public override string ToString()
        {
            return $"[{GetType().Name}] Name: {Name}, Status: {(Status ? "ON" : "OFF")}";
        }
    }
}
