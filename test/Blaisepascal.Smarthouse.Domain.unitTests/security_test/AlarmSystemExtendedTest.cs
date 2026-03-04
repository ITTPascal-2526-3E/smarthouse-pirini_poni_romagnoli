using System;
using Xunit;
using BlaisePascal.SmartHouse.Domain.Security.SecurityDevices;
using BlaisePascal.SmartHouse.Domain.Security.SecurityAbstraction;

namespace Blaisepascal.Smarthouse.Domain.unitTests.security_test
{
    public class AlarmSystemExtendedTest
    {
        // Helper method to create a default AlarmSystem
        private static AlarmSystem CreateAlarm()
        {
            return new AlarmSystem("Verisure", "V-Pro");
        }

        // The test verifies that ResetIntrusion clears all intrusion flags
        [Fact]
        public void ResetIntrusion_ClearsAllFlags()
        {
            // Arrange
            var alarm = CreateAlarm();
            alarm.DetectIntrusion();

            // Act
            alarm.ResetIntrusion();

            // Assert
            Assert.False(alarm.Intrusion);
            Assert.False(alarm.IntrusionNotification);
            Assert.False(alarm.Signal);
        }

        // The test verifies that ActivateSignal sets Signal to true
        [Fact]
        public void ActivateSignal_SetsSignalToTrue()
        {
            // Arrange
            var alarm = CreateAlarm();

            // Act
            alarm.ActivateSignal();

            // Assert
            Assert.True(alarm.Signal);
        }

        // The test verifies that DeactivateSignal sets Signal to false
        [Fact]
        public void DeactivateSignal_SetsSignalToFalse()
        {
            // Arrange
            var alarm = CreateAlarm();
            alarm.ActivateSignal();

            // Act
            alarm.DeactivateSignal();

            // Assert
            Assert.False(alarm.Signal);
        }

        // The test verifies that IndicateOn sets Notification to true when Status is true
        [Fact]
        public void IndicateOn_SetsNotificationTrue_WhenStatusIsOn()
        {
            // Arrange
            var alarm = CreateAlarm(); // Status is true by default (constructor passes true)

            // Act
            alarm.IndicateOn();

            // Assert
            Assert.True(alarm.Notification);
        }

        // The test verifies that IndicateOff sets Notification to false when Status is false
        [Fact]
        public void IndicateOff_SetsNotificationFalse_WhenStatusIsOff()
        {
            // Arrange
            var alarm = CreateAlarm();
            alarm.ToggleOff(); // Set Status to false

            // Act
            alarm.IndicateOff();

            // Assert
            Assert.False(alarm.Notification);
        }

        // The test verifies that IndicateOn does NOT change Notification when Status is false
        [Fact]
        public void IndicateOn_DoesNotChangeNotification_WhenStatusIsOff()
        {
            // Arrange
            var alarm = CreateAlarm();
            alarm.ToggleOff(); // Status = false

            // Act
            alarm.IndicateOn();

            // Assert — Notification should remain false (default)
            Assert.False(alarm.Notification);
        }

        // The test verifies that IndicateOff does NOT change Notification when Status is true
        [Fact]
        public void IndicateOff_DoesNotChangeNotification_WhenStatusIsOn()
        {
            // Arrange
            var alarm = CreateAlarm(); // Status = true
            alarm.IndicateOn(); // Set Notification to true

            // Act
            alarm.IndicateOff(); // Should NOT clear because Status is true

            // Assert
            Assert.True(alarm.Notification);
        }

        // The test verifies that IndicateOnPerSignal sets Notification when Signal is on
        [Fact]
        public void IndicateOnPerSignal_SetsNotification_WhenSignalIsOn()
        {
            // Arrange
            var alarm = CreateAlarm();
            alarm.ActivateSignal();

            // Act
            alarm.IndicateOnPerSignal();

            // Assert
            Assert.True(alarm.Notification);
        }

        // The test verifies that IndicateOffPerSignal clears Notification when Signal is off
        [Fact]
        public void IndicateOffPerSignal_ClearsNotification_WhenSignalIsOff()
        {
            // Arrange
            var alarm = CreateAlarm();
            alarm.ActivateSignal();
            alarm.IndicateOnPerSignal(); // Notification = true
            alarm.DeactivateSignal();    // Signal = false

            // Act
            alarm.IndicateOffPerSignal();

            // Assert
            Assert.False(alarm.Notification);
        }

        // The test verifies that Arm sets IsArmed to true
        [Fact]
        public void Arm_SetsIsArmedToTrue()
        {
            // Arrange
            var alarm = CreateAlarm();
            alarm.Disarm();

            // Act
            alarm.Arm();

            // Assert
            Assert.True(alarm.IsArmed);
        }

        // The test verifies that Disarm sets IsArmed to false
        [Fact]
        public void Disarm_SetsIsArmedToFalse()
        {
            // Arrange
            var alarm = CreateAlarm();

            // Act
            alarm.Disarm();

            // Assert
            Assert.False(alarm.IsArmed);
        }

        // The test verifies that TriggerAlarm fires the OnAlarm event
        [Fact]
        public void TriggerAlarm_FiresOnAlarmEvent()
        {
            // Arrange
            var alarm = CreateAlarm();
            string? receivedName = null;
            string? receivedMessage = null;

            alarm.OnAlarm += (name, msg) =>
            {
                receivedName = name;
                receivedMessage = msg;
            };

            // Act
            alarm.TriggerAlarm();

            // Assert
            Assert.NotNull(receivedName);
            Assert.NotNull(receivedMessage);
            Assert.Contains("SIREN", receivedMessage);
        }
    }
}
