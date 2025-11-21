using System;
public class Door
{
    private Guid Id { get; }
    public string Name { get; set; } //name that the user can assign to the device
    public bool IsOpen { get; set; } //True if the door is open
    public bool IsLock { get; set; } //True if the door is lock

    public Door(string name)
    {
        this.Name = name;
        this.IsOpen = false;
        this.IsLock = true; 
        this.Id = Guid.NewGuid();
    }

    public void OpenDoor()
    {
        IsOpen = true;
    }

    public void CloseDoor()
    {
        IsOpen = false;
    }

    public void LockDoor()
    {
        IsLock = true;
    }

    public void OpenDoorWithKey()
    {
        IsLock = false;
    }
}





