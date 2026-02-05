using System;

namespace BlaisePascal.SmartHouse.Domain.ValueObjects
{
    // Encapsulates a temperature value with validation and conversion logic
    public record Temperature
    {
        // Value in Celsius
        public double Value { get; }

        // Physical limit: nothing can be colder than absolute zero
        public const double AbsoluteZero = -273.15;

        // Validates that the temperature is physically possible
        public Temperature(double value)
        {
            if (value < AbsoluteZero)
            {
                throw new ArgumentException($"Temperature cannot be below absolute zero ({AbsoluteZero}°C). Provided: {value}");
            }
            Value = value;
        }

        // Returns the value padded with the Celsius symbol
        public override string ToString()
        {
            return $"{Value}°C";
        }

        // Allows the record to be treated like a double in calculations
        public static implicit operator double(Temperature temp)
        {
            return temp.Value;
        }

        // Allows creating a Temperature from a double value
        public static explicit operator Temperature(double value)
        {
            return new Temperature(value);
        }

        // Comparison operators to allow natural syntax (temp1 > temp2)
        public static bool operator <(Temperature a, Temperature b)
        {
            return a.Value < b.Value;
        }

        public static bool operator >(Temperature a, Temperature b)
        {
            return a.Value > b.Value;
        }

        public static bool operator <=(Temperature a, Temperature b)
        {
            return a.Value <= b.Value;
        }

        public static bool operator >=(Temperature a, Temperature b)
        {
            return a.Value >= b.Value;
        }

        // Arithmetic operators to allow increments/decrements
        public static Temperature operator +(Temperature a, double change)
        {
            return new Temperature(a.Value + change);
        }

        public static Temperature operator -(Temperature a, double change)
        {
            return new Temperature(a.Value - change);
        }
    }
}
