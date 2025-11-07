using System;

public class HeatPump
{
    //Heat pump modes
    public enum ModeOption { Heating, Cooling, Fan, Dry }
	public int CurrentTemperature { get; private set; }
	public ModeOption Mode { get; private set; }

	//Mode setting
	public void SetMode(ModeOption mode)
	{
		Mode = mode;
	}

    //Temperature setting based on mode and temperature wanted
    public void SetTemperature(int temperature)
	{
		if (Mode == ModeOption.Heating && temperature < CurrentTemperature)
		{
			return;
		}
		if (Mode == ModeOption.Cooling && temperature > CurrentTemperature)
		{
			return;
		}
		if (Mode == ModeOption.Fan)
		{
			CurrentTemperature -= temperature;
			return;
		}
		if (Mode == ModeOption.Dry)
		{
			CurrentTemperature += temperature;
			return;
		}
		CurrentTemperature = temperature;
	}
	
}
