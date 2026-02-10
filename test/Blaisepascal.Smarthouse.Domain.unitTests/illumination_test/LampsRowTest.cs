using BlaisePascal.SmartHouse.Domain;
using BlaisePascal.SmartHouse.Domain.Illumination.LampTypes;
using BlaisePascal.SmartHouse.Domain.Illumination.LampOptions;
using BlaisePascal.SmartHouse.Domain.Illumination.LampCompositions;
using BlaisePascal.SmartHouse.Domain.Illumination.LampAbstraction;
using BlaisePascal.SmartHouse.Domain.ValueObjects;

namespace Blaisepascal.Smarthouse.Domain.unitTests.illumination_test
{
    public class LampsRowTest
    {
        // Helper method to create a LampsRow instance
        private LampsRow CreateDevice()
        {
            return new LampsRow();
        }

        // Helper method to create a standard Lamp
        private Lamp CreateStandardLamp()
        {
            return new Lamp(
                10,
                ColorOption.NeutralWhite,
                "Std-1",
                "BrandA",
                EnergyClass.A,
                "Standard Lamp");
        }

        // Helper method to create an EcoLamp
        private EcoLamp CreateEcoLamp()
        {
            return new EcoLamp(
                15,
                ColorOption.Green,
                "Eco-1",
                "BrandB",
                EnergyClass.A,
                "Eco Lamp");
        }

        // The test checks that adding lamps increases the total count
        [Fact]
        public void AddLamp_IncreasesTotalCount()
        {
            // Arrange
            var device = CreateDevice();

            // Act
            device.AddLamp(CreateStandardLamp());
            device.AddLamp(CreateEcoLamp()); // EcoLamp added with the same method

            // Assert
            Assert.Equal(2, device.GetLampsCount());
        }

        // The test checks that GetLampAtIndex returns the correct object
        [Fact]
        public void GetLampAtIndex_ReturnsCorrectObject()
        {
            // Arrange
            var device = CreateDevice();
            var lamp1 = CreateStandardLamp();
            device.AddLamp(lamp1);

            // Act
            var retrieved = device.GetLampAtIndex(0);

            // Assert
            Assert.Same(lamp1, retrieved);
        }

        // The test checks that RemoveLampAtIndex removes the item and shifts the list
        [Fact]
        public void RemoveLampAtIndex_RemovesItemFromList()
        {
            // Arrange
            var device = CreateDevice();
            device.AddLamp(CreateStandardLamp()); // index 0
            var ecoLamp = CreateEcoLamp();        // index 1
            device.AddLamp(ecoLamp);

            // Act
            device.RemoveLampAtIndex(0);

            // Assert
            Assert.Equal(1, device.GetLampsCount());
            Assert.Same(ecoLamp, device.GetLampAtIndex(0));
        }

        // The test checks that ClearAllLamps empties the list
        [Fact]
        public void ClearAllLamps_EmptiesTheList()
        {
            // Arrange
            var device = CreateDevice();
            device.AddLamp(CreateStandardLamp());
            device.AddLamp(CreateStandardLamp());

            // Act
            device.ClearAllLamps();

            // Assert
            Assert.Equal(0, device.GetLampsCount());
        }

        // The test checks that TurnOnAllLamps turns on all lamps
        [Fact]
        public void TurnOnAllLamps_TurnsOnAllLamps()
        {
            // Arrange
            var device = CreateDevice();
            var normalLamp = CreateStandardLamp();
            var ecoLamp = CreateEcoLamp();

            device.AddLamp(normalLamp);
            device.AddLamp(ecoLamp);

            // Act
            device.TurnOnAllLamps();

            // Assert
            Assert.True(normalLamp.IsOn);
            Assert.True(ecoLamp.IsOn);
            Assert.Equal(2, device.GetOnLampsCount());
        }

        // The test checks that TurnOffAllLamps turns off all lamps
        [Fact]
        public void TurnOffAllLamps_TurnsOffAllLamps()
        {
            // Arrange
            var device = CreateDevice();
            var normalLamp = CreateStandardLamp();
            normalLamp.ToggleOn();

            device.AddLamp(normalLamp);

            // Act
            device.TurnOffAllLamps();

            // Assert
            Assert.False(normalLamp.IsOn);
            Assert.Equal(0, device.GetOnLampsCount());
        }

        // The test checks that SetLuminosityAllLamps changes brightness for every lamp
        [Fact]
        public void SetLuminosityAllLamps_ChangesBrightnessForEveryone()
        {
            // Arrange
            var device = CreateDevice();
            var lamp1 = CreateStandardLamp();
            var lamp2 = CreateEcoLamp();

            device.AddLamp(lamp1);
            device.AddLamp(lamp2);
            device.TurnOnAllLamps();

            // Act
            device.SetLuminosityAllLamps(55);

            // Assert
            Assert.Equal(55, lamp1.CurrentLuminosity.Value);
            Assert.Equal(55, lamp2.CurrentLuminosity.Value);
        }

        // The test checks that TurnOnLampAtIndex turns on only the selected lamp
        [Fact]
        public void TurnOnLampAtIndex_TurnsOnOnlySelectedLamp()
        {
            // Arrange
            var device = CreateDevice();
            var lamp1 = CreateStandardLamp(); // index 0
            var lamp2 = CreateStandardLamp(); // index 1

            device.AddLamp(lamp1);
            device.AddLamp(lamp2);

            // Act
            device.TurnOnLampAtIndex(1);

            // Assert
            Assert.False(lamp1.IsOn);
            Assert.True(lamp2.IsOn);
        }

        // The test checks that index-based methods do not throw with invalid indices
        [Fact]
        public void IndexMethods_DoNotCrash_WithInvalidIndex()
        {
            // Arrange
            var device = CreateDevice();
            device.AddLamp(CreateStandardLamp());

            // Act
            var ex = Record.Exception(() =>
            {
                device.TurnOnLampAtIndex(99);       // out of range
                device.TurnOffLampAtIndex(-1);      // negative index
                device.SetLuminosityAtIndex(50, 80);
                device.RemoveLampAtIndex(100);
            });

            // Assert
            Assert.Null(ex);
        }

        // The test checks that UpdateAllEcoLamps triggers dimming only on EcoLamps
        [Fact]
        public void UpdateAllEcoLamps_TriggersDimmingOnEcoLamps()
        {
            // Arrange
            var device = CreateDevice();
            var ecoLamp = CreateEcoLamp();
            var normalLamp = CreateStandardLamp();

            device.AddLamp(ecoLamp);
            device.AddLamp(normalLamp);
            device.TurnOnAllLamps();

            // Act
            device.UpdateAllEcoLamps(DateTime.UtcNow.AddMinutes(10));

            // Assert
            Assert.Equal(30, ecoLamp.CurrentLuminosity.Value);
            Assert.Equal(100, normalLamp.CurrentLuminosity.Value);
        }

        // The test checks that RegisterPresenceAllEcoLamps restores brightness on EcoLamps
        [Fact]
        public void RegisterPresenceAllEcoLamps_RestoresBrightness()
        {
            // Arrange
            var device = CreateDevice();
            var ecoLamp = CreateEcoLamp();

            device.AddLamp(ecoLamp);
            device.TurnOnAllLamps();
            device.UpdateAllEcoLamps(DateTime.UtcNow.AddMinutes(10));
            Assert.Equal(30, ecoLamp.CurrentLuminosity.Value);

            // Act
            device.RegisterPresenceAllEcoLamps();

            // Assert
            Assert.Equal(100, ecoLamp.CurrentLuminosity.Value);
        }
    }
}
