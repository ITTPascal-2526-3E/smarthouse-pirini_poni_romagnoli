using System;

public class Device
{
	public string Name;
	public bool Status ;
    public DateTime CreatedAtUTC { get; set; }
    public DateTime LastmodifiedAtUtc;
    public Device(string name, bool status )
	{
        CreatedAtUTC = DateTime.Now;
        Name = name;
        Status = status;

    }


}
