using BlaisePascal.SmartHouse.Domain.Illumination.LampTypes;
using BlaisePascal.SmartHouse.Domain.Illumination.LampOptions;
using BlaisePascal.SmartHouse.Domain.Illumination.LampAbstraction;
using BlaisePascal.SmartHouse.Domain.ValueObjects;
using Xunit;

namespace Blaisepascal.Smarthouse.Domain.unitTests.illumination_test
{
    public class LampExtendedTest
    {
        // Helper to create a default lamp
        private static Lamp CreateDefaultLamp()
        {
            return new Lamp(10, ColorOption.WarmWhite, "XU-200", "SmartTech", EnergyClass.A, "Desk Lamp");
        }

        // The test verifies that GetLuminosity returns 0 when lamp is OFF
        [Fact]
        public void GetLuminosity_ReturnsZero_WhenLampIsOff()
        {
            // Arrange
            var lamp = CreateDefaultLamp();

            // Act
            var luminosity = lamp.GetLuminosity();

            // Assert
            Assert.Equal(0, luminosity.Value);
        }

        // The test verifies that GetLuminosity returns 100 when lamp is ON at full brightness
        [Fact]
        public void GetLuminosity_Returns100_WhenLampIsOnAtFull()
        {
            // Arrange
            var lamp = CreateDefaultLamp();
            lamp.ToggleOn();

            // Act
            var luminosity = lamp.GetLuminosity();

            // Assert
            Assert.Equal(100, luminosity.Value);
        }

        // The test verifies that GetLuminosity reflects dimmed value
        [Fact]
        public void GetLuminosity_ReturnsDimmedValue_AfterSetLuminosity()
        {
            // Arrange
            var lamp = CreateDefaultLamp();
            lamp.ToggleOn();
            lamp.SetLuminosity(42);

            // Act
            var luminosity = lamp.GetLuminosity();

            // Assert
            Assert.Equal(42, luminosity.Value);
        }

        // The test verifies that the IDimmable.Luminosity property getter works correctly
        [Fact]
        public void IDimmable_Luminosity_Get_ReturnsCurrentLuminosity()
        {
            // Arrange
            var lamp = CreateDefaultLamp();
            lamp.ToggleOn();
            lamp.SetLuminosity(75);

            // Act — access through the IDimmable interface
            IDimmable dimmable = lamp;
            var luminosity = dimmable.Luminosity;

            // Assert
            Assert.Equal(75, luminosity.Value);
        }

        // The test verifies that the IDimmable.Luminosity property setter updates CurrentLuminosity
        [Fact]
        public void IDimmable_Luminosity_Set_UpdatesCurrentLuminosity()
        {
            // Arrange
            var lamp = CreateDefaultLamp();
            lamp.ToggleOn();

            // Act — set through the IDimmable interface
            IDimmable dimmable = lamp;
            dimmable.Luminosity = new Luminosity(60);

            // Assert
            Assert.Equal(60, lamp.CurrentLuminosity.Value);
        }

        // The test verifies that Luminosity setter and getter round-trip correctly
        [Fact]
        public void Luminosity_SetterAndGetter_RoundTrip()
        {
            // Arrange
            var lamp = CreateDefaultLamp();
            lamp.ToggleOn();

            // Act
            lamp.Luminosity = new Luminosity(33);

            // Assert
            Assert.Equal(33, lamp.Luminosity.Value);
            Assert.Equal(33, lamp.CurrentLuminosity.Value);
            Assert.Equal(33, lamp.GetLuminosity().Value);
        }
    }
}
