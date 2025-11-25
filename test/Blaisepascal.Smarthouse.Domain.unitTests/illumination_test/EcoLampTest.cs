using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blaisepascal.Smarthouse.Domain.unitTests.illumination_test
{
    public class EcoLampTest
    {
        // --- Helper to create a new lamp ---
        private EcoLamp CreateDefaultEcoLamp()
        {
            return new EcoLamp(10, Lamp.ColorOption.White, "Eco-1", "GreenTech", "A+++", "My Eco Lamp");
        }

        
        // 1. TESTS FOR AUTOMATIC DIMMING (SAVING ENERGY)
        

        [Fact]
        public void Update_LowersBrightness_After5MinutesOfNoMovement()
        {
            // Arrange
            var lamp = CreateDefaultEcoLamp();
            lamp.TurnOn(); // The lamp must be ON

            // We pretend that 6 minutes have passed since the last movement
            // (The limit is 5 minutes)
            var timeInFuture = DateTime.Now.AddMinutes(6);

            // Act
            // We tell the lamp: "Update yourself, the time is now 'timeInFuture'"
            lamp.Update(timeInFuture);

            // Assert
            // The brightness should go down to 30 because nobody moved
            Assert.Equal(30, lamp.LuminosityPercentage);
        }

        [Fact]
        public void Update_KeepsFullBrightness_Before5Minutes()
        {
            // Arrange
            var lamp = CreateDefaultEcoLamp();
            lamp.TurnOn();

            // We pretend only 4 minutes have passed
            // (The limit is 5 minutes, so it is too early to dim)
            var timeInFuture = DateTime.Now.AddMinutes(4);

            // Act
            lamp.Update(timeInFuture);

            // Assert
            // The brightness should stay at 100
            Assert.Equal(100, lamp.LuminosityPercentage);
        }

        [Fact]
        public void RegisterPresence_RestoresFullBrightness_IfLow()
        {
            // Arrange
            var lamp = CreateDefaultEcoLamp();
            lamp.TurnOn();

            // We force the lamp to go into "saving mode" (low brightness)
            lamp.Update(DateTime.Now.AddMinutes(10));

            // Check that it is actually low (30)
            Assert.Equal(30, lamp.LuminosityPercentage);

            // Act
            // Someone moves in the room!
            lamp.RegisterPresence();

            // Assert
            // The lamp should go back to full power (100)
            Assert.Equal(100, lamp.LuminosityPercentage);
        }

        
        // 2. TESTS FOR THE TIMER (SCHEDULING)
        

        [Fact]
        public void Schedule_TurnOn_WorksCorrectly()
        {
            // Arrange
            var lamp = CreateDefaultEcoLamp();
            var now = DateTime.Now;
            var timeToTurnOn = now.AddHours(1); // Set timer for 1 hour later

            // We set the timer to turn ON
            lamp.Schedule(timeToTurnOn, null);

            // Act 
            // We simulate that time has passed and we are now 1 second after the timer
            lamp.Update(timeToTurnOn.AddSeconds(1));

            // Assert
            Assert.True(lamp.IsOn, "The lamp should be ON now.");
            Assert.Null(lamp.ScheduledOn); // The timer should be cleared/empty now
        }

        [Fact]
        public void Schedule_TurnOff_WorksCorrectly()
        {
            // Arrange
            var lamp = CreateDefaultEcoLamp();
            lamp.TurnOn(); // Start with lamp ON
            var now = DateTime.Now;
            var timeToTurnOff = now.AddHours(1); // Set timer for 1 hour later

            // We set the timer to turn OFF
            lamp.Schedule(null, timeToTurnOff);

            // Act
            // We simulate that time has passed
            lamp.Update(timeToTurnOff.AddSeconds(1));

            // Assert
            Assert.False(lamp.IsOn, "The lamp should be OFF now.");
            Assert.Null(lamp.ScheduledOff); // The timer should be cleared/empty
        }

        
        // 3. TEST FOR TOTAL USAGE TIME
        

        [Fact]
        public async Task TotalOnTime_CountsTimeCorrectly()
        {
            // Note: This test uses "Task.Delay" to wait for real time to pass.

            // Arrange
            var lamp = CreateDefaultEcoLamp();

            // At the start, the total time should be zero
            Assert.Equal(TimeSpan.Zero, lamp.TotalOnTime);

            // Act 
            lamp.TurnOn();
            await Task.Delay(100); // We wait for 100 milliseconds (real time)
            lamp.TurnOff();

            // Assert
            // Check if the lamp counted some time (more than 80ms to be safe)
            Assert.True(lamp.TotalOnTime.TotalMilliseconds >= 80, "It should count the time it was ON.");

            // We save the current total time
            var timeAfterFirstRun = lamp.TotalOnTime;

            // Act again
            lamp.TurnOn();
            await Task.Delay(100); // Wait again
            lamp.TurnOff();

            // Assert again
            // The total time should be higher now because we added more time
            Assert.True(lamp.TotalOnTime > timeAfterFirstRun, "The total time should increase.");
        }
    }
}

