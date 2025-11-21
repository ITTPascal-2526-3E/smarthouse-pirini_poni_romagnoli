using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blaisepascal.Smarthouse.Domain.unitTests.illumination_test
{
    public class LampTest
    {
        // Dati di test fissi e semplici
        private const int Power = 10;
        private const Lamp.ColorOption Color = Lamp.ColorOption.WarmWhite;
        private const string Model = "XU-200";
        private const string Brand = "SmartTech";
        private const string EnergyClass = "A++";
        private const string Name = "Lampada da scrivania";

        private Lamp CreateDefaultLamp()
        {
            return new Lamp(Power, Color, Model, Brand, EnergyClass, Name);
        }

        [Fact]
        public void Constructor_InitialStateIsOffAndZeroLuminosity()
        {
            // Arrange
            var lamp = CreateDefaultLamp();

            // Assert
            Assert.False(lamp.IsOn);
            Assert.Equal(0, lamp.LuminosityPercentage);
        }

        [Fact]
        public void Constructor_SetsFixedPropertiesCorrectly()
        {
            // Arrange
            var lamp = CreateDefaultLamp();

            // Assert
            Assert.Equal(Power, lamp.Power);
            Assert.Equal(Brand, lamp.Brand);
            Assert.Equal(Model, lamp.Model);
            Assert.Equal(EnergyClass, lamp.EnergyClass);
            Assert.Equal(Name, lamp.name);
        }
    }
}
