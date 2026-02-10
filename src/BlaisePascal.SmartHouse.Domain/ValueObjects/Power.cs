using System;

namespace BlaisePascal.SmartHouse.Domain.ValueObjects
{
    // Represents the power intensity level of the heat pump (0-100)
    public class Power
    {
        // Internal value of the power intensity
        public int Value { get; }

        public const int MinValue = 0;
        public const int MaxValue = 100;

        // Constructor with validation to ensure the value is within range
        public Power(int value)
        {
            if (value < MinValue || value > MaxValue)
            {
                throw new ArgumentException($"Power must be between {MinValue} and {MaxValue}. Provided: {value}");
            }
            Value = value;
        }

        // Custom string representation
        public override string ToString()
        {
            return $"{Value}%";
        }

        // Allows using the Power object directly as an int
        public static implicit operator int(Power power)//see temperature for specs
        {
            return power.Value;
        }

        // Allows creating a Power object from an int value (explicit cast)
        public static explicit operator Power(int value)//see temperature for specs
        {
            return new Power(value);
        }
    }
}
