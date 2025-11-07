using System;

public class TwoLampsDevice()
{
	private Lamp MyLamp;
	private EcoLamp MyEcolamp;

	public TwoLampsDevice(Lamp mylamp, EcoLamp myecolamp)
	{
		MyLamp = mylamp;
		MyEcolamp = myecolamp;
	}

	public void TurnOnMyLamp()
	{
		MyLamp.TurnOn();
	}

	public void TurnOffMyLamp()
	{ 
		MyLamp.TurnOf();
	}

	public void SetMyLampLuminosity(int percentage)
	{
		MyLamp.SetLuminosity(percentage);
    }

	public void TurnOnMyEcoLamp()
	{
		MyEcolamp.TurnOn();
    }

	public void TurnOffMyEcoLamp()
	{ 
		MyEcolamp.TurnOf();
    }

	public void
}
