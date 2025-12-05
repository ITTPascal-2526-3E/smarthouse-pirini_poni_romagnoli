using BlaisePascal.SmartHouse.Domain;
using BlaisePascal.SmartHouse.Domain.@enum;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Blaisepascal.Smarthouse.Domain.unitTests.illumination_test
{
    public class EcoLampTest
    {
        // Helper to create a new EcoLamp
        private EcoLamp CreateDefaultEcoLamp()
        {
            return new EcoLamp(10, ColorOption.White, "Eco-1", "GreenTech", EnergyClass.A_plus_plus_plus, "My Eco Lamp");
        }

        // The test checks that brightness is lowered after 5 minutes of no presence.
        [Fact]
        public void Update_LowersBrightness_After5MinutesOfNoMovement()
        {
            // Arrange
            var lamp = CreateDefaultEcoLamp();
            lamp.TurnOn();

            var timeInFuture = DateTime.UtcNow.AddMinutes(6);

            // Act
            lamp.Update(timeInFuture);

            // Assert
            Assert.Equal(30, lamp.LuminosityPercentage);
        }

        // The test checks that brightness stays full before the 5 minutes timeout
        [Fact]
        public void Update_KeepsFullBrightness_Before5Minutes()
        {
            // Arrange
            var lamp = CreateDefaultEcoLamp();
            lamp.TurnOn();

            var timeInFuture = DateTime.UtcNow.AddMinutes(4);

            // Act
            lamp.Update(timeInFuture);

            // Assert
            Assert.Equal(100, lamp.LuminosityPercentage);
        }

        // The test checks that RegisterPresence restores full brightness from dim mode
        [Fact]
        public void RegisterPresence_RestoresFullBrightness_IfLow()
        {
            // Arrange
            var lamp = CreateDefaultEcoLamp();
            lamp.TurnOn();

            lamp.Update(DateTime.UtcNow.AddMinutes(10));
            Assert.Equal(30, lamp.LuminosityPercentage);

            // Act
            lamp.RegisterPresence();

            // Assert
            Assert.Equal(100, lamp.LuminosityPercentage);
        }

        // The test checks that scheduled ON time turns the lamp ON and clears the schedule
        [Fact]
        public void Schedule_TurnOn_WorksCorrectly()
        {
            // Arrange
            var lamp = CreateDefaultEcoLamp();
            var now = DateTime.UtcNow;
            var timeToTurnOn = now.AddHours(1);

            lamp.Schedule(timeToTurnOn, null);

            // Act
            lamp.Update(timeToTurnOn.AddSeconds(1));

            // Assert
            Assert.True(lamp.IsOn);
            Assert.Null(lamp.ScheduledOn);
        }

        // The test checks that scheduled OFF time turns the lamp OFF and clears the schedule
        [Fact]
        public void Schedule_TurnOff_WorksCorrectly()
        {
            // Arrange
            var lamp = CreateDefaultEcoLamp();
            lamp.TurnOn();
            var now = DateTime.UtcNow;
            var timeToTurnOff = now.AddHours(1);

            lamp.Schedule(null, timeToTurnOff);

            // Act
            lamp.Update(timeToTurnOff.AddSeconds(1));

            // Assert
            Assert.False(lamp.IsOn);
            Assert.Null(lamp.ScheduledOff);
        }

        // The test checks that TotalOnTime accumulates the time when the lamp is ON
        [Fact]
        public async Task TotalOnTime_CountsTimeCorrectly()
        {
            // Arrange
            var lamp = CreateDefaultEcoLamp();
            Assert.Equal(TimeSpan.Zero, lamp.TotalOnTime);

            // Act: first run
            lamp.TurnOn();
            await Task.Delay(100);
            lamp.TurnOff();

            // Assert: some time should be accumulated
            Assert.True(lamp.TotalOnTime.TotalMilliseconds >= 80);

            var timeAfterFirstRun = lamp.TotalOnTime;

            // Act: second run
            lamp.TurnOn();
            await Task.Delay(100);
            lamp.TurnOff();

            // Assert: total time should be greater than after first run
            Assert.True(lamp.TotalOnTime > timeAfterFirstRun);
        }
    }
}
