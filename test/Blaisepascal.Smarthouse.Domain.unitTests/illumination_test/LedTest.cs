using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blaisepascal.Smarthouse.Domain.unitTests
{
    public class LedTest
    {
        private Led CreateLed()
        {
            return new Led("Living Room LED", false, BlaisePascal.SmartHouse.Domain.illumination.ColorOption.White);
        }

        [Fact]
        public void ChangeColor_ChangesColorOption()
        {
            // Arrange
            var led = CreateLed();
            // Act
            led.ChangeColor(BlaisePascal.SmartHouse.Domain.illumination.ColorOption.Blue);
            // Assert
            Assert.Equal(BlaisePascal.SmartHouse.Domain.illumination.ColorOption.Blue, led.colorOption);
        }

        [Fact]
        public void SetLightIntensity_UpdatesIntensity()
        {
            // Arrange
            var led = CreateLed();
            // Act
            led.SetLightIntensity(85);
            // Assert
            Assert.Equal(85, led.LightIntensity);
        }

        [Fact]
        public void Led_InitializesWithDefaultIntensity()
        {
            // Arrange & Act
            var led = CreateLed();
            // Assert
            Assert.Equal(70, led.LightIntensity);
        }

        
    }
}
