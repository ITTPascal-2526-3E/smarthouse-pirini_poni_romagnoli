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

        // --- Test per TurnOn e TurnOff ---

        [Fact]
        public void TurnOn_SetsIsOnTrueAndLuminosityToMax()
        {
            // Arrange
            var lamp = CreateDefaultLamp();

            // Act
            lamp.TurnOn();

            // Assert
            Assert.True(lamp.IsOn, "La lampada dovrebbe essere accesa (IsOn = true)");
            Assert.Equal(100, lamp.LuminosityPercentage);
        }

        [Fact]
        public void TurnOff_SetsIsOnFalseAndLuminosityToZero()
        {
            // Arrange
            var lamp = CreateDefaultLamp();
            lamp.TurnOn(); // Prima la accendiamo

            // Act
            lamp.TurnOff();

            // Assert
            Assert.False(lamp.IsOn, "La lampada dovrebbe essere spenta (IsOn = false)");
            Assert.Equal(0, lamp.LuminosityPercentage);
        }

        // --- Test per SetLuminosity (Logica e Confini) ---

        [Fact]
        public void SetLuminosity_UpdatesValue_WhenLampIsOnAndValueIsValid()
        {
            // Arrange
            var lamp = CreateDefaultLamp();
            lamp.TurnOn(); // Deve essere accesa per cambiare luminosità

            // Act
            lamp.SetLuminosity(50);

            // Assert
            Assert.Equal(50, lamp.LuminosityPercentage);
        }

        [Fact]
        public void SetLuminosity_DoesNothing_WhenLampIsOff()
        {
            // Arrange
            var lamp = CreateDefaultLamp();
            // La lampada è spenta di default (IsOn = false)

            // Act
            lamp.SetLuminosity(50);

            // Assert
            Assert.Equal(0, lamp.LuminosityPercentage); // Non deve essere cambiata
        }

        [Fact]
        public void SetLuminosity_DoesNothing_WhenValueIsNegative()
        {
            // Arrange
            var lamp = CreateDefaultLamp();
            lamp.TurnOn(); // Luminosità iniziale 100

            // Act
            lamp.SetLuminosity(-10);

            // Assert
            Assert.Equal(100, lamp.LuminosityPercentage); // Deve rimanere al valore precedente
        }

        [Fact]
        public void SetLuminosity_DoesNothing_WhenValueIsGreaterThan100()
        {
            // Arrange
            var lamp = CreateDefaultLamp();
            lamp.TurnOn(); // Luminosità iniziale 100
            lamp.SetLuminosity(50); // La portiamo a 50 per verificare che non cambi

            // Act
            lamp.SetLuminosity(150);

            // Assert
            Assert.Equal(50, lamp.LuminosityPercentage); // Deve rimanere 50
        }

        // --- Test per le proprietà modificabili e generate ---

        [Fact]
        public void Color_CanBeChanged()
        {
            // Arrange
            var lamp = CreateDefaultLamp();

            // Act
            lamp.Color = Lamp.ColorOption.Blue;

            // Assert
            Assert.Equal(Lamp.ColorOption.Blue, lamp.Color);
        }

        [Fact]
        public void LampId_IsGeneratedAutomatically()
        {
            // Arrange
            var lamp = CreateDefaultLamp();

            // Assert
            Assert.NotEqual(Guid.Empty, lamp.LampId);
        }




        // 1. Test dei Limiti Esatti 
        // per assicurarsi che i limiti siano inclusivi.
        [Fact]
        public void SetLuminosity_AllowsBoundaryValues_0_And_100()
        {
            // Arrange
            var lamp = CreateDefaultLamp();
            lamp.TurnOn();

            // Act & Assert - Limite inferiore
            lamp.SetLuminosity(0);
            Assert.Equal(0, lamp.LuminosityPercentage);

            // Act & Assert - Limite superiore
            lamp.SetLuminosity(100);
            Assert.Equal(100, lamp.LuminosityPercentage);
        }

        // 2. Test di "Reset" Luminosità

        [Fact]
        public void TurnOn_ResetsLuminosityTo100_IfAlreadyOnAndDimmed()
        {
            // Arrange
            var lamp = CreateDefaultLamp();
            lamp.TurnOn();
            lamp.SetLuminosity(30); // La portiamo al 30%

            // Act
            lamp.TurnOn(); // La riaccendiamo (o resettiamo)

            // Assert
            Assert.Equal(100, lamp.LuminosityPercentage);
        }

        // 3. Test di Unicità ID
        // verifica che due lampade diverse abbiano ID diversi.
        [Fact]
        public void Constructor_GeneratesUniqueIdsForDifferentInstances()
        {
            // Arrange
            var lamp1 = CreateDefaultLamp();
            var lamp2 = CreateDefaultLamp();

            // Assert
            Assert.NotEqual(lamp1.LampId, lamp2.LampId);
        }

        // 4. Test del Setter del Nome
        // Hai testato il costruttore, ma non il fatto che il nome possa essere cambiato dopo.
        [Fact]
        public void Name_CanBeChanged()
        {
            // Arrange
            var lamp = CreateDefaultLamp();
            string newName = "Luce Cucina";

            // Act
            lamp.name = newName;

            // Assert
            Assert.Equal(newName, lamp.name);
        }
    }

}
