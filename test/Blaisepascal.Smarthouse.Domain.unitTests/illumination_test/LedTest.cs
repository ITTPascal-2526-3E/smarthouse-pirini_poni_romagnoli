using System;
using BlaisePascal.SmartHouse.Domain.Illumination.LampTypes;
using BlaisePascal.SmartHouse.Domain.Illumination.LampOptions;
using BlaisePascal.SmartHouse.Domain.Illumination.LampCompositions;
using BlaisePascal.SmartHouse.Domain.Illumination.LampAbstraction;
using BlaisePascal.SmartHouse.Domain.ValueObjects;
using Xunit;

namespace Blaisepascal.Smarthouse.Domain.unitTests.illumination_test
{
    public class LedTest
    {
        private Led CreateLed()
        {
            return new Led(1, ColorOption.NeutralWhite, "ModelX", "BrandY", EnergyClass.A);
        }

        [Fact]
        public void ChangeColor_ChangesColorOption()
        {
            // Arrange
            var led = CreateLed();
            // Act
            led.ChangeColor(ColorOption.Blue);
            // Assert
            Assert.Equal(ColorOption.Blue, led.Color);
        }

        [Fact]
        public void SetLightIntensity_UpdatesIntensity()
        {
            // Arrange
            var led = CreateLed();
            led.ToggleOn();
            // Act
            led.SetLightIntensity(85);
            // Assert
            Assert.Equal(85, led.CurrentLuminosity.Value);
        }

        [Fact]
        public void Led_InitializesWithZeroLuminosity()
        {
            // Arrange & Act
            var led = CreateLed();
            // Assert
            Assert.Equal(0, led.CurrentLuminosity.Value);
        }
    }
}
