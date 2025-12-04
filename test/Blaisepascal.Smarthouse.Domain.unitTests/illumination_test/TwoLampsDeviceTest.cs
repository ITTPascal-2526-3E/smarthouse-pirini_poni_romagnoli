using BlaisePascal.SmartHouse.Domain;
using BlaisePascal.SmartHouse.Domain.@enum;
using System;
using Xunit;

namespace Blaisepascal.Smarthouse.Domain.unitTests.illumination_test
{
    public class TwoLampsDeviceTest
    {
        // Helper method to create a new TwoLampsDevice
        private TwoLampsDevice CreateDevice()
        {
            return new TwoLampsDevice("Test Two Lamps");
        }

        // Helper method to create a standard Lamp
        private Lamp CreateStandardLamp()
        {
            return new Lamp(10, ColorOption.White, "Std-1", "BrandA", EnergyClass.A_plus_plus, "Standard Lamp");
        }

        // Helper method to create an EcoLamp
        private EcoLamp CreateEcoLamp()
        {
            return new EcoLamp(15, ColorOption.Green, "Eco-1", "BrandB", EnergyClass.A_plus_plus_plus, "Eco Lamp");
        }

        // Test that adding lamps increases the total count up to the maximum of two
        [Fact]
        public void AddLamp_IncreasesTotalCount_UpToTwo()
        {
            var device = CreateDevice();

            device.AddLamp(CreateStandardLamp());
            device.AddLamp(CreateEcoLamp());

            Assert.Equal(2, device.GetLampsCount());
        }

        // Test that adding more than two lamps does not exceed the maximum capacity
        [Fact]
        public void AddLamp_IgnoresExtraLampsBeyondCapacity()
        {
            var device = CreateDevice();

            device.AddLamp(CreateStandardLamp());
            device.AddLamp(CreateEcoLamp());
            device.AddLamp(CreateStandardLamp()); // Third lamp should be ignored

            Assert.Equal(2, device.GetLampsCount());
        }

        // Test that GetLampAtIndex returns the correct lamp instance
        [Fact]
        public void GetLampAtIndex_ReturnsCorrectLamp()
        {
            var device = CreateDevice();
            var lamp1 = CreateStandardLamp();
            var lamp2 = CreateEcoLamp();

            device.AddLamp(lamp1);
            device.AddLamp(lamp2);

            var first = device.GetLampAtIndex(0);
            var second = device.GetLampAtIndex(1);

            Assert.Same(lamp1, first);
            Assert.Same(lamp2, second);
        }

        // Test that RemoveLampAtIndex removes the lamp and shifts the remaining one
        [Fact]
        public void RemoveLampAtIndex_RemovesLampAndShiftsRemaining()
        {
            var device = CreateDevice();
            var lamp1 = CreateStandardLamp();
            var ecoLamp = CreateEcoLamp();

            device.AddLamp(lamp1);    // index 0
            device.AddLamp(ecoLamp);  // index 1

            device.RemoveLampAtIndex(0);

            Assert.Equal(1, device.GetLampsCount());
            Assert.Same(ecoLamp, device.GetLampAtIndex(0));
        }

        // Test that ClearAllLamps empties the device and turns its status OFF
        [Fact]
        public void ClearAllLamps_EmptiesDeviceAndTurnsStatusOff()
        {
            var device = CreateDevice();
            device.AddLamp(CreateStandardLamp());
            device.AddLamp(CreateEcoLamp());

            device.TurnOnBothLamps();
            Assert.True(device.Status);

            device.ClearAllLamps();

            Assert.Equal(0, device.GetLampsCount());
            Assert.False(device.Status);
        }

        // Test that TurnOnBothLamps turns on all lamps and sets device status to ON
        [Fact]
        public void TurnOnBothLamps_TurnsOnAllLampsAndDevice()
        {
            var device = CreateDevice();
            var lamp1 = CreateStandardLamp();
            var ecoLamp = CreateEcoLamp();

            device.AddLamp(lamp1);
            device.AddLamp(ecoLamp);

            device.TurnOnBothLamps();

            Assert.True(lamp1.IsOn);
            Assert.True(ecoLamp.IsOn);
            Assert.Equal(2, device.GetOnLampsCount());
            Assert.True(device.Status);
        }

        // Test that TurnOffBothLamps turns off all lamps and sets device status to OFF
        [Fact]
        public void TurnOffBothLamps_TurnsOffAllLampsAndDevice()
        {
            var device = CreateDevice();
            var lamp1 = CreateStandardLamp();
            var ecoLamp = CreateEcoLamp();

            device.AddLamp(lamp1);
            device.AddLamp(ecoLamp);

            device.TurnOnBothLamps();
            device.TurnOffBothLamps();

            Assert.False(lamp1.IsOn);
            Assert.False(ecoLamp.IsOn);
            Assert.Equal(0, device.GetOnLampsCount());
            Assert.False(device.Status);
        }

        // Test that SetLuminosityBothLamps applies the same brightness to all lamps
        [Fact]
        public void SetLuminosityBothLamps_ChangesBrightnessForAllLamps()
        {
            var device = CreateDevice();
            var lamp1 = CreateStandardLamp();
            var ecoLamp = CreateEcoLamp();

            device.AddLamp(lamp1);
            device.AddLamp(ecoLamp);

            device.TurnOnBothLamps();
            device.SetLuminosityBothLamps(60);

            Assert.Equal(60, lamp1.LuminosityPercentage);
            Assert.Equal(60, ecoLamp.LuminosityPercentage);
        }

        // Test that TurnOnLampAtIndex turns on only the selected lamp
        [Fact]
        public void TurnOnLampAtIndex_TurnsOnOnlySelectedLamp()
        {
            var device = CreateDevice();
            var lamp1 = CreateStandardLamp(); // index 0
            var lamp2 = CreateStandardLamp(); // index 1

            device.AddLamp(lamp1);
            device.AddLamp(lamp2);

            device.TurnOnLampAtIndex(1);

            Assert.False(lamp1.IsOn);
            Assert.True(lamp2.IsOn);
            Assert.True(device.Status);
        }

        // Test that index-based methods do not throw on invalid indices
        [Fact]
        public void IndexMethods_HandleInvalidIndicesWithoutThrowing()
        {
            var device = CreateDevice();
            device.AddLamp(CreateStandardLamp());

            var ex = Record.Exception(() =>
            {
                device.TurnOnLampAtIndex(99);
                device.TurnOffLampAtIndex(-1);
                device.SetLuminosityAtIndex(42, 50);
                device.RemoveLampAtIndex(100);
            });

            Assert.Null(ex);
        }

        // Test that UpdateBothEcoLamps delegates Update only to EcoLamp instances
        [Fact]
        public void UpdateBothEcoLamps_AffectsOnlyEcoLamps()
        {
            var device = CreateDevice();
            var ecoLamp = CreateEcoLamp();
            var normalLamp = CreateStandardLamp();

            device.AddLamp(ecoLamp);
            device.AddLamp(normalLamp);

            device.TurnOnBothLamps();

            device.UpdateBothEcoLamps(DateTime.UtcNow.AddMinutes(10));

            Assert.Equal(30, ecoLamp.LuminosityPercentage);   // EcoLamp should dim
            Assert.Equal(100, normalLamp.LuminosityPercentage); // Normal lamp unchanged
        }

        // Test that RegisterPresenceBothEcoLamps restores full brightness on EcoLamps
        [Fact]
        public void RegisterPresenceBothEcoLamps_RestoresEcoLampBrightness()
        {
            var device = CreateDevice();
            var ecoLamp = CreateEcoLamp();

            device.AddLamp(ecoLamp);
            device.TurnOnBothLamps();

            device.UpdateBothEcoLamps(DateTime.UtcNow.AddMinutes(10));
            Assert.Equal(30, ecoLamp.LuminosityPercentage);

            device.RegisterPresenceBothEcoLamps();

            Assert.Equal(100, ecoLamp.LuminosityPercentage);
        }

        // Test that ScheduleBothEcoLamps forwards schedule to EcoLamps only
        [Fact]
        public void ScheduleBothEcoLamps_ForwardsScheduleToEcoLamps()
        {
            var device = CreateDevice();
            var ecoLamp = CreateEcoLamp();
            var normalLamp = CreateStandardLamp();

            device.AddLamp(ecoLamp);
            device.AddLamp(normalLamp);

            var now = DateTime.UtcNow;
            var onTime = now.AddMinutes(1);
            var offTime = now.AddMinutes(10);

            device.ScheduleBothEcoLamps(onTime, offTime);

            Assert.Equal(onTime, ecoLamp.ScheduledOn);
            Assert.Equal(offTime, ecoLamp.ScheduledOff);
        }
    }
}
