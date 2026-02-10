using System;

namespace BlaisePascal.SmartHouse.Domain.ValueObjects
{
    // Ensures that a device name is valid (non-empty)
    public class DeviceName
    {
        // String value of the name
        public string Value { get; }

        // Validates that the name contains at least one non-space character
        public DeviceName(string value)
        {
            if (string.IsNullOrWhiteSpace(value))//IsNullOrWhiteSpace means that the string is null or contains only whitespace characters, it is an already existing method in c#
            {
                throw new ArgumentException("Device name cannot be empty or whitespace.");
            }
            Value = value;
        }

        public override string ToString()
        {
            return Value;
        }

        // Allows implicit conversion between DeviceName and string for zero-friction usage
        public static implicit operator string(DeviceName name)//see temperature for specs
        {
            return name.Value;
        }

        public static implicit operator DeviceName(string value)//see temperature for specs
        {
            return new DeviceName(value);
        }
    }
}
