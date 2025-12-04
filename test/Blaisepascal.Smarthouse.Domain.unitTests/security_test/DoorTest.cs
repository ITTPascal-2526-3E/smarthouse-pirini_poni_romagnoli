namespace Blaisepascal.Smarthouse.Domain.unitTests.security_test;
public class DoorTest
{
    //Testing the door opening
    [Fact]
    public void doorOpened()
    {
        Door door = new Door("Front Door", false);
        door.OpenDoor();
        Assert.True(door.Status);
    }
    //Testing the door closing
    [Fact]
    public void doorClosed()
    {
        Door door = new Door("Front Door", false);
        door.CloseDoor();
        Assert.False(door.Status);
    }
    //Testing the door locking
    [Fact]
    public void doorLocked()
    {
        Door door = new Door("Front Door", false);
        door.LockDoor();
        Assert.True(door.IsLock);
    }
    //Testing the door unlocking
    [Fact]
    public void doorUnlocked()
    {
        Door door = new Door("Front Door", false);
        door.OpenDoorWithKey();
        Assert.False(door.IsLock);
    }
    //Testing that when door is created, it's closed AND locked
    [Fact]
    public void doorInitialState()
    {
        Door door = new Door("Front Door", false);
        Assert.False(door.Status);
        Assert.True(door.IsLock);
    }
    //Testing door's name assignment
    [Fact]
    public void doorNameAssignment()
    {
        string doorName = "Back Door";
        Door door = new Door(doorName, false);
        Assert.Equal(doorName, door.Name);
    }
    //Pulga <3
}
