using System;
public class Door:Device
{
    private Guid Id { get; }
     
    
    public bool IsLock { get; set; } //True if the door is lock

    public Door(string name, bool status)
        : base(name, status)

    {
        base.Name = name;
        base.Status = status;
        IsLock = true; 
        Id = Guid.NewGuid();
    }

    public void OpenDoor()
    {
        if (!IsLock)
        {
            base.Status = true;
            LastmodifiedAtUtc = DateTime.Now;
        }

    }

    public void CloseDoor()
    {
        Status = false;
        LastmodifiedAtUtc = DateTime.Now;
    }

    public void LockDoor()
    {
        IsLock = true;
        LastmodifiedAtUtc = DateTime.Now;
    }

    public void UnlockDoor()
    {
        IsLock = false;
        LastmodifiedAtUtc = DateTime.Now;
    }

    public void OpenDoorWithKey()
    {
        base.Status = true;
        IsLock = false;
        LastmodifiedAtUtc = DateTime.Now;
    }

    public void CloseDoorWithKey()
    {
        base.Status = false;
        IsLock = true;
        LastmodifiedAtUtc = DateTime.Now;
    }

}





