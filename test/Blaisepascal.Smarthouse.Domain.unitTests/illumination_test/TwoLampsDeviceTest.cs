using System;
using Xunit;
using BlaisePascal.SmartHouse.Domain;
using BlaisePascal.SmartHouse.Domain.illumination;
using BlaisePascal.SmartHouse.Domain.ValueObjects;

namespace Blaisepascal.Smarthouse.Domain.unitTests.illumination_test
{
    public class TwoLampsDeviceTest
    {
        // Helper method to create a TwoLampsDevice
        private TwoLampsDevice CreateDevice()
        {
            return new TwoLampsDevice("Desk Lamps");
        }

        // Helper method to create a standard Lamp
        private Lamp CreateStandardLamp(string name = "Standard Lamp")
        {
            return new Lamp(
                10,
                ColorOption.White,
                "Std-1",
                "BrandA",
                EnergyClass.A,
                name);
        }

        // Helper method to create an EcoLamp
        private EcoLamp CreateEcoLamp(string name = "Eco Lamp")
        {
            return new EcoLamp(
                12,
                ColorOption.WarmWhite,
                "Eco-1",
                "BrandB",
                EnergyClass.A_plus_plus,
                name);
        }

        // The test verifies that adding one lamp increases the count to 1
        [Fact]
        public void AddLamp_IncreasesCount_UpToTwo()
        {
            // Arrange
            var device = CreateDevice();

            // Act
            device.AddLamp(CreateStandardLamp("Lamp A"));

            // Assert
            Assert.Equal(1, device.GetLampsCount());

            // Act again
            device.AddLamp(CreateStandardLamp("Lamp B"));

            // Assert
            Assert.Equal(2, device.GetLampsCount());
        }

        // The test verifies that a third lamp is ignored (max two lamps)
        [Fact]
        public void AddLamp_IgnoresThirdLamp()
        {
            // Arrange
            var device = CreateDevice();

            device.AddLamp(CreateStandardLamp("Lamp A"));
            device.AddLamp(CreateStandardLamp("Lamp B"));

            // Act
            device.AddLamp(CreateStandardLamp("Lamp C"));

            // Assert
            Assert.Equal(2, device.GetLampsCount());
            Assert.Equal("Lamp A", device.GetLampAtIndex(0)!.Name);
            Assert.Equal("Lamp B", device.GetLampAtIndex(1)!.Name);
        }

        // The test verifies that GetLampAtIndex returns the correct lamp
        [Fact]
        public void GetLampAtIndex_ReturnsCorrectLamp()
        {
            // Arrange
            var device = CreateDevice();
            var lampA = CreateStandardLamp("Lamp A");
            var lampB = CreateStandardLamp("Lamp B");

            device.AddLamp(lampA);
            device.AddLamp(lampB);

            // Act
            var first = device.GetLampAtIndex(0);
            var second = device.GetLampAtIndex(1);
            var invalid = device.GetLampAtIndex(2);

            // Assert
            Assert.Same(lampA, first);
            Assert.Same(lampB, second);
            Assert.Null(invalid);
        }

        // The test verifies that RemoveLampAtIndex removes the right lamp
        [Fact]
        public void RemoveLampAtIndex_RemovesLampAndUpdatesCount()
        {
            // Arrange
            var device = CreateDevice();
            var lampA = CreateStandardLamp("Lamp A");
            var lampB = CreateStandardLamp("Lamp B");

            device.AddLamp(lampA);
            device.AddLamp(lampB);

            // Act
            device.RemoveLampAtIndex(0);

            // Assert
            Assert.Equal(1, device.GetLampsCount());
            Assert.Same(lampB, device.GetLampAtIndex(1));
            Assert.Null(device.GetLampAtIndex(0));
        }

        // The test verifies that ClearAllLamps removes both lamps and turns device OFF
        [Fact]
        public void ClearAllLamps_RemovesAllAndTurnsOffDevice()
        {
            // Arrange
            var device = CreateDevice();
            device.AddLamp(CreateStandardLamp());
            device.AddLamp(CreateStandardLamp());
            device.TurnOnBothLamps();

            // Act
            device.ClearAllLamps();

            // Assert
            Assert.Equal(0, device.GetLampsCount());
            Assert.False(device.Status);
        }

        // The test verifies that TurnOnBothLamps turns on both lamps and sets device status to ON
        [Fact]
        public void TurnOnBothLamps_TurnsOnLampsAndDevice()
        {
            // Arrange
            var device = CreateDevice();
            var lampA = CreateStandardLamp("Lamp A");
            var lampB = CreateStandardLamp("Lamp B");

            device.AddLamp(lampA);
            device.AddLamp(lampB);

            // Act
            device.TurnOnBothLamps();

            // Assert
            Assert.True(lampA.IsOn);
            Assert.True(lampB.IsOn);
            Assert.True(device.Status);
            Assert.Equal(2, device.GetOnLampsCount());
        }

        // The test verifies that TurnOffBothLamps turns off both lamps and sets device status to OFF
        [Fact]
        public void TurnOffBothLamps_TurnsOffLampsAndDevice()
        {
            // Arrange
            var device = CreateDevice();
            var lampA = CreateStandardLamp("Lamp A");
            var lampB = CreateStandardLamp("Lamp B");

            device.AddLamp(lampA);
            device.AddLamp(lampB);
            device.TurnOnBothLamps();

            // Act
            device.TurnOffBothLamps();

            // Assert
            Assert.False(lampA.IsOn);
            Assert.False(lampB.IsOn);
            Assert.False(device.Status);
            Assert.Equal(0, device.GetOnLampsCount());
        }

        // The test verifies that SetLuminosityBothLamps changes brightness for both lamps
        [Fact]
        public void SetLuminosityBothLamps_ChangesBrightnessForBoth()
        {
            // Arrange
            var device = CreateDevice();
            var lampA = CreateStandardLamp("Lamp A");
            var lampB = CreateStandardLamp("Lamp B");

            device.AddLamp(lampA);
            device.AddLamp(lampB);
            device.TurnOnBothLamps();

            // Act
            device.SetLuminosityBothLamps(60);

            // Assert
            Assert.Equal(60, lampA.CurrentLuminosity.Value);
            Assert.Equal(60, lampB.CurrentLuminosity.Value);
            Assert.True(device.Status);
        }

        // The test verifies that TurnOnLampAtIndex affects only the selected lamp
        [Fact]
        public void TurnOnLampAtIndex_TurnsOnOnlySelectedLamp()
        {
            // Arrange
            var device = CreateDevice();
            var lampA = CreateStandardLamp("Lamp A");
            var lampB = CreateStandardLamp("Lamp B");

            device.AddLamp(lampA);
            device.AddLamp(lampB);

            // Act
            device.TurnOnLampAtIndex(1);

            // Assert
            Assert.False(lampA.IsOn);
            Assert.True(lampB.IsOn);
            Assert.True(device.Status);
        }

        // The test verifies that TurnOffLampAtIndex affects only the selected lamp
        [Fact]
        public void TurnOffLampAtIndex_TurnsOffOnlySelectedLamp()
        {
            // Arrange
            var device = CreateDevice();
            var lampA = CreateStandardLamp("Lamp A");
            var lampB = CreateStandardLamp("Lamp B");

            device.AddLamp(lampA);
            device.AddLamp(lampB);
            device.TurnOnBothLamps();

            // Act
            device.TurnOffLampAtIndex(0);

            // Assert
            Assert.False(lampA.IsOn);
            Assert.True(lampB.IsOn);
            Assert.True(device.Status); // device still active because one lamp is ON
        }

        // The test verifies that SetLuminosityAtIndex changes brightness only for the selected lamp
        [Fact]
        public void SetLuminosityAtIndex_ChangesBrightnessOnlyForSelectedLamp()
        {
            // Arrange
            var device = CreateDevice();
            var lampA = CreateStandardLamp("Lamp A");
            var lampB = CreateStandardLamp("Lamp B");

            device.AddLamp(lampA);
            device.AddLamp(lampB);
            device.TurnOnBothLamps();

            // Act
            device.SetLuminosityAtIndex(1, 30);

            // Assert
            Assert.Equal(100, lampA.CurrentLuminosity.Value);
            Assert.Equal(30, lampB.CurrentLuminosity.Value);
        }

        // The test verifies that index-based methods do not throw for invalid indices
        [Fact]
        public void IndexMethods_DoNotThrow_OnInvalidIndex()
        {
            // Arrange
            var device = CreateDevice();
            device.AddLamp(CreateStandardLamp());

            // Act
            var ex = Record.Exception(() =>
            {
                device.TurnOnLampAtIndex(5);
                device.TurnOffLampAtIndex(-1);
                device.SetLuminosityAtIndex(3, 50);
                device.RemoveLampAtIndex(10);
            });

            // Assert
            Assert.Null(ex);
        }

        // The test verifies that UpdateBothEcoLamps delegates to EcoLamp.Update and triggers dimming
        [Fact]
        public void UpdateBothEcoLamps_TriggersDimmingOnEcoLamps()
        {
            // Arrange
            var device = CreateDevice();
            var ecoLamp = CreateEcoLamp("Eco Desk Lamp");
            var normalLamp = CreateStandardLamp("Standard");

            device.AddLamp(ecoLamp);
            device.AddLamp(normalLamp);

            device.TurnOnBothLamps();

            // Act
            var futureTime = DateTime.UtcNow.AddMinutes(10);
            device.UpdateBothEcoLamps(futureTime);

            // Assert
            Assert.Equal(30, ecoLamp.CurrentLuminosity.Value);   // EcoLamp dims
            Assert.Equal(100, normalLamp.CurrentLuminosity.Value); // Normal lamp not affected by Eco update
        }

        // The test verifies that RegisterPresenceBothEcoLamps restores brightness on EcoLamp
        [Fact]
        public void RegisterPresenceBothEcoLamps_RestoresBrightness()
        {
            // Arrange
            var device = CreateDevice();
            var ecoLamp = CreateEcoLamp("Eco Desk Lamp");

            device.AddLamp(ecoLamp);
            device.TurnOnBothLamps();

            device.UpdateBothEcoLamps(DateTime.UtcNow.AddMinutes(10));
            Assert.Equal(30, ecoLamp.CurrentLuminosity.Value);

            // Act
            device.RegisterPresenceBothEcoLamps();

            // Assert
            Assert.Equal(100, ecoLamp.CurrentLuminosity.Value);
        }

        // The test verifies that ScheduleBothEcoLamps configures scheduling on EcoLamps
        [Fact]
        public void ScheduleBothEcoLamps_SetsScheduleOnEcoLamps()
        {
            // Arrange
            var device = CreateDevice();
            var ecoLampA = CreateEcoLamp("Eco A");
            var ecoLampB = CreateEcoLamp("Eco B");

            device.AddLamp(ecoLampA);
            device.AddLamp(ecoLampB);

            DateTime onTime = DateTime.UtcNow.AddHours(1);
            DateTime offTime = DateTime.UtcNow.AddHours(2);

            // Act
            device.ScheduleBothEcoLamps(onTime, offTime);

            // Assert
            Assert.Equal(onTime, ecoLampA.ScheduledOn);
            Assert.Equal(offTime, ecoLampA.ScheduledOff);
            Assert.Equal(onTime, ecoLampB.ScheduledOn);
            Assert.Equal(offTime, ecoLampB.ScheduledOff);
        }
    }
}
