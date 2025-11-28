using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using System.Collections.Generic;

namespace Blaisepascal.Smarthouse.Domain.unitTests.illumination_test 
{ 
    public class TwoLampsDeviceTest
    {

        //  Helper Methods to create instances 

        private LampsRow CreateDevice()
        {
            return new LampsRow();
        }

        private Lamp CreateStandardLamp()
        {
            return new Lamp(10, Lamp.ColorOption.White, "Std-1", "BrandA", "A", "Standard Lamp");
        }

        private EcoLamp CreateEcoLamp()
        {
            return new EcoLamp(15, Lamp.ColorOption.Green, "Eco-1", "BrandB", "A++", "Eco Lamp");
        }


        // 1. TEST GESTIONE LISTA (ADD / REMOVE / COUNT)


        [Fact]
        public void AddLamp_IncreasesTotalCount()
        {
            // Arrange
            var device = CreateDevice();

            // Act
            device.addLamp(CreateStandardLamp());
            device.addEcoLamp(CreateEcoLamp());

            // Assert
            Assert.Equal(2, device.getLampsCount());
        }

        [Fact]
        public void GetLampAtIndex_ReturnsCorrectObject()
        {
            // Arrange
            var device = CreateDevice();
            var lamp1 = CreateStandardLamp();
            device.addLamp(lamp1);

            // Act
            var retrieved = device.getLampAtIndex(0);

            // Assert
            Assert.Same(lamp1, retrieved); // Verify it's exactly the same object
        }

        [Fact]
        public void RemoveLampAtIndex_RemovesItemFromList()
        {
            // Arrange
            var device = CreateDevice();
            device.addLamp(CreateStandardLamp()); // Index 0
            device.addEcoLamp(CreateEcoLamp());   // Index 1

            // Act
            device.RemoveLampAtIndex(0);

            // Assert
            Assert.Equal(1, device.getLampsCount());
            // The EcoLamp should now be at index 0
            Assert.IsType<EcoLamp>(device.getLampAtIndex(0));
        }

        [Fact]
        public void ClearAllLamps_EmptiesTheList()
        {
            // Arrange
            var device = CreateDevice();
            device.addLamp(CreateStandardLamp());
            device.addLamp(CreateStandardLamp());

            // Act
            device.ClearAllLamps();

            // Assert
            Assert.Equal(0, device.getLampsCount());
        }


        // 2. TEST COMANDI GLOBALI (TUTTE LE LAMPADE)


        [Fact]
        public void TurnOnAllLamps_TurnsOnBothTypes()
        {
            // Arrange
            var device = CreateDevice();
            var normalLamp = CreateStandardLamp();
            var ecoLamp = CreateEcoLamp();

            device.addLamp(normalLamp);
            device.addEcoLamp(ecoLamp);

            // Act
            device.TurnOnAllLamps();

            // Assert
            Assert.True(normalLamp.IsOn, "Standard lamp should be ON");
            Assert.True(ecoLamp.IsOn, "Eco lamp should be ON");
            Assert.Equal(2, device.getONLampsCount());
        }

        [Fact]
        public void TurnOffAllLamps_TurnsOffBothTypes()
        {
            // Arrange
            var device = CreateDevice();
            var normalLamp = CreateStandardLamp();
            normalLamp.TurnOn(); // Start ON

            device.addLamp(normalLamp);

            // Act
            device.TurnOffAllLamps();

            // Assert
            Assert.False(normalLamp.IsOn);
            Assert.Equal(0, device.getONLampsCount());
        }

        [Fact]
        public void SetLuminosityAllLamps_ChangesBrightnessForEveryone()
        {
            // Arrange
            var device = CreateDevice();
            var lamp1 = CreateStandardLamp();
            var lamp2 = CreateEcoLamp();

            device.addLamp(lamp1);
            device.addEcoLamp(lamp2);

            device.TurnOnAllLamps(); // Must be ON to change luminosity

            // Act
            device.SetLuminosityAllLamps(55);

            // Assert
            Assert.Equal(55, lamp1.LuminosityPercentage);
            Assert.Equal(55, lamp2.LuminosityPercentage);
        }


        // 3. TEST COMANDI SINGOLI (INDEX) E EDGE CASES


        [Fact]
        public void TurnOnLampAtIndex_TurnsOnOnlySelectedLamp()
        {
            // Arrange
            var device = CreateDevice();
            var lamp1 = CreateStandardLamp(); // Index 0
            var lamp2 = CreateStandardLamp(); // Index 1

            device.addLamp(lamp1);
            device.addLamp(lamp2);

            // Act
            device.turnonlampsatindex(1); // Turn on only the second one

            // Assert
            Assert.False(lamp1.IsOn, "Lamp 0 should remain OFF");
            Assert.True(lamp2.IsOn, "Lamp 1 should be ON");
        }

        [Fact]
        public void IndexMethods_DoNotCrash_WithInvalidIndex()
        {
            // Arrange
            var device = CreateDevice();
            device.addLamp(CreateStandardLamp());

            // Act & Assert (Should not throw exception)
            try
            {
                device.turnonlampsatindex(99); // Index out of bounds
                device.turnofflampsatindex(-1); // Negative index
                device.RemoveLampAtIndex(99);
            }
            catch (Exception)
            {
                Assert.Fail("The code should handle invalid indexes gracefully without crashing.");
            }
        }


        // 4. TEST SPECIFICI PER ECOLAMP (DELEGAZIONE)


        [Fact]
        public void UpdateAllEcoLamps_TriggersDimmingOnEcoLamps()
        {
            // Verify that the container correctly passes the Update command to EcoLamps

            // Arrange
            var device = CreateDevice();
            var ecoLamp = CreateEcoLamp();
            var normalLamp = CreateStandardLamp();

            device.addEcoLamp(ecoLamp);
            device.addLamp(normalLamp);

            device.TurnOnAllLamps();

            // Act
            // Simulate 10 minutes passing
            device.UpdateAllEcoLamps(DateTime.Now.AddMinutes(10));

            // Assert
            Assert.Equal(30, ecoLamp.LuminosityPercentage); // Should dim
            Assert.Equal(100, normalLamp.LuminosityPercentage); // Standard lamp is ignored by UpdateAllEcoLamps
        }

        [Fact]
        public void RegisterPresenceAllEcoLamps_RestoresBrightness()
        {
            // Arrange
            var device = CreateDevice();
            var ecoLamp = CreateEcoLamp();
            device.addEcoLamp(ecoLamp);
            device.TurnOnAllLamps();

            // Force dimming
            device.UpdateAllEcoLamps(DateTime.Now.AddMinutes(10));
            Assert.Equal(30, ecoLamp.LuminosityPercentage);

            // Act
            device.RegisterPresenceAllEcoLamps();

            // Assert
            Assert.Equal(100, ecoLamp.LuminosityPercentage);
        }

    }
}

