using System;
using Xunit;
using BlaisePascal.SmartHouse.Domain.Security.SecurityDevices;
using BlaisePascal.SmartHouse.Domain.Security.SecurityAbstraction;

namespace Blaisepascal.Smarthouse.Domain.unitTests.security_test
{
    public class DoorExtendedTest
    {
        // Helper method to create a default door (closed, locked)
        private static Door CreateDoor()
        {
            return new Door("Test Door", false);
        }

        // The test verifies that LockDoor does NOT change the open/closed Status
        [Fact]
        public void LockDoor_DoesNotChangeStatus()
        {
            // Arrange
            var door = CreateDoor();
            door.UnlockDoor();
            door.OpenDoor(); // Status = true (open)

            // Act
            door.LockDoor();

            // Assert — door is still open but now locked
            Assert.True(door.IsLocked);
            Assert.True(door.Status); // Status remains open
        }

        // The test verifies that UnlockDoor does NOT change the open/closed Status
        [Fact]
        public void UnlockDoor_DoesNotChangeStatus()
        {
            // Arrange
            var door = CreateDoor(); // Status = false (closed), IsLocked = true

            // Act
            door.UnlockDoor();

            // Assert — door is still closed but now unlocked
            Assert.False(door.IsLocked);
            Assert.False(door.Status); // Status remains closed
        }

        // The test verifies that OpenDoor correctly updates Status to true (open)
        [Fact]
        public void OpenDoor_SetsStatusToTrue_WhenUnlocked()
        {
            // Arrange
            var door = CreateDoor();
            door.UnlockDoor();

            // Act
            door.OpenDoor();

            // Assert
            Assert.True(door.Status); // Open
            Assert.False(door.IsLocked);
        }

        // The test verifies that CloseDoor correctly updates Status to false (closed)
        [Fact]
        public void CloseDoor_SetsStatusToFalse()
        {
            // Arrange
            var door = CreateDoor();
            door.UnlockDoor();
            door.OpenDoor();

            // Act
            door.CloseDoor();

            // Assert
            Assert.False(door.Status); // Closed
        }

        // The test verifies that OpenDoorWithKey unlocks AND sets Status = open
        [Fact]
        public void OpenDoorWithKey_SetsCorrectStatusAndLock()
        {
            // Arrange
            var door = CreateDoor(); // locked, closed

            // Act
            door.OpenDoorWithKey();

            // Assert
            Assert.True(door.Status);     // Open
            Assert.False(door.IsLocked);  // Unlocked
        }

        // The test verifies that CloseDoorWithKey closes AND locks correctly
        [Fact]
        public void CloseDoorWithKey_SetsCorrectStatusAndLock()
        {
            // Arrange
            var door = CreateDoor();
            door.OpenDoorWithKey(); // Open + unlocked

            // Act
            door.CloseDoorWithKey();

            // Assert
            Assert.False(door.Status);   // Closed
            Assert.True(door.IsLocked);  // Locked
        }

        // The test verifies that TriggerAlarm fires the OnAlarm event for doors
        [Fact]
        public void TriggerAlarm_FiresOnAlarmEvent()
        {
            // Arrange
            var door = CreateDoor();
            string? receivedName = null;
            string? receivedMessage = null;

            door.OnAlarm += (name, msg) =>
            {
                receivedName = name;
                receivedMessage = msg;
            };

            // Act
            door.TriggerAlarm();

            // Assert
            Assert.NotNull(receivedName);
            Assert.NotNull(receivedMessage);
            Assert.Contains("breached", receivedMessage);
        }

        // The test verifies that CloseDoorWithKey → OpenDoorWithKey round-trip works
        [Fact]
        public void FullRoundTrip_OpenClose_WithKey()
        {
            // Arrange
            var door = CreateDoor();

            // Act & Assert: open with key
            door.OpenDoorWithKey();
            Assert.True(door.Status);
            Assert.False(door.IsLocked);

            // Act & Assert: close with key
            door.CloseDoorWithKey();
            Assert.False(door.Status);
            Assert.True(door.IsLocked);

            // Act & Assert: open again with key
            door.OpenDoorWithKey();
            Assert.True(door.Status);
            Assert.False(door.IsLocked);
        }
    }
}
