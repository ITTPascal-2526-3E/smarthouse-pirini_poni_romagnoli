using System;
using Xunit;
using BlaisePascal.SmartHouse.Domain.Security.SecurityDevices;
using BlaisePascal.SmartHouse.Domain.Security.SecurityAbstraction;

namespace Blaisepascal.Smarthouse.Domain.unitTests.security_test
{
    public class DoorTest
    {
        // Helper method to create a default door
        private static Door CreateDoor()
        {
            return new Door("Front Door", false);
        }

        // The test verifies that a locked door does NOT open using OpenDoor()
        [Fact]
        public void OpenDoor_DoesNotOpen_WhenLocked()
        {
            // Arrange
            var door = CreateDoor();    // Locked by default

            // Act
            door.OpenDoor();

            // Assert
            Assert.False(door.Status);  // Still closed
            Assert.True(door.IsLocked);   // Still locked
        }

        // The test verifies that OpenDoor works correctly when the door is unlocked
        [Fact]
        public void OpenDoor_OpensDoor_WhenUnlocked()
        {
            // Arrange
            var door = CreateDoor();
            door.UnlockDoor();

            // Act
            door.OpenDoor();

            // Assert
            Assert.True(door.Status);
            Assert.False(door.IsLocked);
        }

        // The test verifies that CloseDoor closes the door
        [Fact]
        public void CloseDoor_ClosesDoor()
        {
            // Arrange
            var door = CreateDoor();
            door.UnlockDoor();
            door.OpenDoor();

            // Act
            door.CloseDoor();

            // Assert
            Assert.False(door.Status);
        }

        // The test verifies that LockDoor locks the door
        [Fact]
        public void LockDoor_LocksDoor()
        {
            // Arrange
            var door = CreateDoor();
            door.UnlockDoor();

            // Act
            door.LockDoor();

            // Assert
            Assert.True(door.IsLocked);
        }

        // The test verifies that UnlockDoor unlocks the door
        [Fact]
        public void UnlockDoor_UnlocksDoor()
        {
            // Arrange
            var door = CreateDoor();

            // Act
            door.UnlockDoor();

            // Assert
            Assert.False(door.IsLocked);
        }

        // The test verifies that OpenDoorWithKey unlocks AND opens the door
        [Fact]
        public void OpenDoorWithKey_UnlocksAndOpensDoor()
        {
            // Arrange
            var door = CreateDoor();

            // Act
            door.OpenDoorWithKey();

            // Assert
            Assert.True(door.Status);    // Open
            Assert.False(door.IsLocked);   // Unlocked
        }

        // The test verifies that CloseDoorWithKey closes AND locks the door
        [Fact]
        public void CloseDoorWithKey_ClosesAndLocksDoor()
        {
            // Arrange
            var door = CreateDoor();
            door.OpenDoorWithKey();

            // Act
            door.CloseDoorWithKey();

            // Assert
            Assert.False(door.Status);   // Closed
            Assert.True(door.IsLocked);    // Locked
        }

        // The test verifies that the constructor sets door closed and locked
        [Fact]
        public void Constructor_SetsInitialStateCorrectly()
        {
            // Arrange & Act
            var door = CreateDoor();

            // Assert
            Assert.False(door.Status);   // Closed
            Assert.True(door.IsLocked);    // Locked
        }

        // The test verifies that the door name is correctly assigned
        [Fact]
        public void Name_IsAssignedCorrectly()
        {
            // Arrange
            string expectedName = "Back Door";

            // Act
            var door = new Door(expectedName, false);

            // Assert
            Assert.Equal(expectedName, door.Name);
        }

        // The test ensures that LastModifiedAtUtc is updated after an action
        [Fact]
        public void DoorAction_UpdatesLastModifiedAtUtc()
        {
            // Arrange
            var door = CreateDoor();
            var before = door.LastModifiedAtUtc;

            // Act
            System.Threading.Thread.Sleep(20); // To ensure time difference
            door.UnlockDoor();

            // Assert
            Assert.True(door.LastModifiedAtUtc > before);
        }
    }

    // Pulga <3
}
