using BlaisePascal.SmartHouse.Domain;
using BlaisePascal.SmartHouse.Domain.illumination;
using BlaisePascal.SmartHouse.Domain.ValueObjects;

namespace Blaisepascal.Smarthouse.Domain.unitTests.illumination_test
{
    public class LampTest
    {
        // Fixed test data
        private const int Power = 10;
        private const ColorOption Color = ColorOption.WarmWhite;
        private const string Model = "XU-200";
        private const string Brand = "SmartTech";
        private const EnergyClass EnergyClassValue = EnergyClass.A_plus_plus;
        private const string Name = "Desk Lamp";

        // Helper to create a default lamp
        private static Lamp CreateDefaultLamp()
        {
            return new Lamp(Power, Color, Model, Brand, EnergyClassValue, Name);
        }

        // The test verifies that a new lamp starts OFF with zero luminosity
        [Fact]
        public void Constructor_InitialStateIsOffAndZeroLuminosity()
        {
            // Arrange
            var lamp = CreateDefaultLamp();

            // Assert
            Assert.False(lamp.IsOn);
            Assert.Equal(0, lamp.CurrentLuminosity.Value);
        }

        // The test verifies that constructor sets all fixed properties correctly
        [Fact]
        public void Constructor_SetsFixedPropertiesCorrectly()
        {
            // Arrange
            var lamp = CreateDefaultLamp();

            // Assert
            Assert.Equal(Power, lamp.Power);
            Assert.Equal(Brand, lamp.Brand);
            Assert.Equal(Model, lamp.Model);
            Assert.Equal(EnergyClassValue, lamp.EnergyEfficiency);
            Assert.Equal(Name, lamp.Name);
        }

        // The test verifies that TurnOn sets IsOn to true and luminosity to max
        [Fact]
        public void TurnOn_SetsIsOnTrueAndLuminosityToMax()
        {
            // Arrange
            var lamp = CreateDefaultLamp();

            // Act
            lamp.ToggleOn();

            // Assert
            Assert.True(lamp.IsOn);
            Assert.Equal(100, lamp.CurrentLuminosity.Value);
        }

        // The test verifies that TurnOff sets IsOn to false and luminosity to zero
        [Fact]
        public void TurnOff_SetsIsOnFalseAndLuminosityToZero()
        {
            // Arrange
            var lamp = CreateDefaultLamp();
            lamp.ToggleOn();

            // Act
            lamp.ToggleOff();

            // Assert
            Assert.False(lamp.IsOn);
            Assert.Equal(0, lamp.CurrentLuminosity.Value);
        }

        // The test checks that SetLuminosity works when lamp is ON and value is valid
        [Fact]
        public void SetLuminosity_UpdatesValue_WhenLampIsOnAndValueIsValid()
        {
            // Arrange
            var lamp = CreateDefaultLamp();
            lamp.ToggleOn();

            // Act
            lamp.SetLuminosity(50);

            // Assert
            Assert.Equal(50, lamp.CurrentLuminosity.Value);
        }

        // The test checks that SetLuminosity does nothing when lamp is OFF
        [Fact]
        public void SetLuminosity_DoesNothing_WhenLampIsOff()
        {
            // Arrange
            var lamp = CreateDefaultLamp();

            // Act
            lamp.SetLuminosity(50);

            // Assert
            Assert.Equal(0, lamp.CurrentLuminosity.Value);
        }

        // The test checks that negative luminosity values are ignored
        [Fact]
        public void SetLuminosity_DoesNothing_WhenValueIsNegative()
        {
            // Arrange
            var lamp = CreateDefaultLamp();
            lamp.ToggleOn();

            // Act
            lamp.SetLuminosity(-10);

            // Assert
            Assert.Equal(100, lamp.CurrentLuminosity.Value);
        }

        // The test checks that values greater than 100 are ignored
        [Fact]
        public void SetLuminosity_DoesNothing_WhenValueIsGreaterThan100()
        {
            // Arrange
            var lamp = CreateDefaultLamp();
            lamp.ToggleOn();
            lamp.SetLuminosity(50);

            // Act
            lamp.SetLuminosity(150);

            // Assert
            Assert.Equal(50, lamp.CurrentLuminosity.Value);
        }

        // The test verifies that boundary values 0 and 100 are accepted
        [Fact]
        public void SetLuminosity_AllowsBoundaryValues_0_And_100()
        {
            // Arrange
            var lamp = CreateDefaultLamp();
            lamp.ToggleOn();

            // Act
            lamp.SetLuminosity(0);

            // Assert
            Assert.Equal(0, lamp.CurrentLuminosity.Value);

            // Act
            lamp.SetLuminosity(100);

            // Assert
            Assert.Equal(100, lamp.CurrentLuminosity.Value);
        }

        // The test verifies that calling TurnOn again resets luminosity to 100
        [Fact]
        public void TurnOn_ResetsLuminosityTo100_IfAlreadyOnAndDimmed()
        {
            // Arrange
            var lamp = CreateDefaultLamp();
            lamp.ToggleOn();
            lamp.SetLuminosity(30);

            // Act
            lamp.ToggleOn();

            // Assert
            Assert.Equal(100, lamp.CurrentLuminosity.Value);
        }

        // The test checks that the Color property can be changed
        [Fact]
        public void Color_CanBeChanged()
        {
            // Arrange
            var lamp = CreateDefaultLamp();

            // Act
            lamp.Color = ColorOption.Blue;

            // Assert
            Assert.Equal(ColorOption.Blue, lamp.Color);
        }


    }
}
