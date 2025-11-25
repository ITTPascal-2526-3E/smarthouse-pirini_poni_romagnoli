namespace Blaisepascal.Smarthouse.Domain.unitTests.security_test;
public class DoorTest
{
    [Fact]
    public void doorOpened()
    {
        Door door = new Door("Front Door");
        door.OpenDoor();
        Assert.True(door.IsOpen);
    }
    [Fact]
    public void doorClosed()
    {
        Door door = new Door("Front Door");
        door.CloseDoor();
        Assert.False(door.IsOpen);
    }
    [Fact]
    public void doorLocked()
    {
        Door door = new Door("Front Door");
        door.LockDoor();
        Assert.True(door.IsLock);
    }
    [Fact]
    public void doorUnlocked()
    {
        Door door = new Door("Front Door");
        door.OpenDoorWithKey();
        Assert.False(door.IsLock);
    }
    [Fact]
    public void doorInitialState()
    {
        Door door = new Door("Front Door");
        Assert.False(door.IsOpen);
        Assert.True(door.IsLock);
    }
    [Fact]
    public void doorNameAssignment()
    {
        string doorName = "Back Door";
        Door door = new Door(doorName);
        Assert.Equal(doorName, door.Name);
    }
}
