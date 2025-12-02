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
        this.IsLock = true; 
        this.Id = Guid.NewGuid();
    }

    public void OpenDoor()
    {
        if (!IsLock)
        {
            base.Status = true;

        }

    }

    public void CloseDoor()
    {
        Status = false;
    }

    public void LockDoor()
    {
        IsLock = true;
    }

    public void UnlockDoor()
    {
        IsLock = false;
    }

    public void OpenDoorWithKey()
    {
        base.Status = true;
        IsLock = false;
    }

    public void CloseDoorWithKey()
    {
        base.Status = false;
        IsLock = true;
        LastmodifiedAtUtc = DateTime.Now;
    }

}





