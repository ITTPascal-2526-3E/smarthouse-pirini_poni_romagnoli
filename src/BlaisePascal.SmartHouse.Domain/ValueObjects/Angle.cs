using System;

namespace BlaisePascal.SmartHouse.Domain.ValueObjects
{
    // Represents the airflow angle of the heat pump (1-90 degrees)
    public record Angle
    {
        // Internal value of the angle
        public int Value { get; }

        public const int MinValue = 1;
        public const int MaxValue = 90;

        // Constructor with validation to ensure the value is within range
        public Angle(int value)
        {
            if (value < MinValue || value > MaxValue)
            {
                throw new ArgumentException($"Angle must be between {MinValue} and {MaxValue}. Provided: {value}");
            }
            Value = value;
        }

        // Custom string representation
        public override string ToString()
        {
            return $"{Value}°";
        }

        // Allows using the Angle object directly as an int
        public static implicit operator int(Angle angle)
        {
            return angle.Value;
        }

        // Allows creating a Angle object from an int value (explicit cast)
        public static explicit operator Angle(int value)
        {
            return new Angle(value);
        }
    }
}
