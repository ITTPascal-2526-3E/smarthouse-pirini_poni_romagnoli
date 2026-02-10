using System;

namespace BlaisePascal.SmartHouse.Domain.ValueObjects
{
    // Represent the brightness level of a lamp as a percentage (0-100)
    public class Luminosity
    {
        // Internal value of the luminosity
        public int Value { get; }

        public const int MinValue = 0;
        public const int MaxValue = 100;

        // Constructor with validation to ensure the value is within range
        public Luminosity(int value)
        {
            if (value < MinValue || value > MaxValue)
            {
                throw new ArgumentException($"Luminosity must be between {MinValue} and {MaxValue}. Provided: {value}");
            }
            Value = value;
        }

        // Custom string representation
        public override string ToString()
        {
            return $"{Value}%";
        }

        // Allows using the Luminosity object directly as an int
        public static implicit operator int(Luminosity lum)//see Temp for specs
        {
            return lum.Value;
        }

        // Allows creating a Luminosity object from an int value (explicit cast)
        public static explicit operator Luminosity(int value)//see Temp for specs
        {
            return new Luminosity(value);
        }
    }
}
