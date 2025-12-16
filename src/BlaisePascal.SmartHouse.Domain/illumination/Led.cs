using BlaisePascal.SmartHouse.Domain;
using System;
using BlaisePascal.SmartHouse.Domain.illumination;

public class Led: Device
{
	public ColorOption colorOption {get; private set;}
	public int LightIntensity { get; private set; } = DEFAULT_INTENSITY ;


	private const int DEFAULT_INTENSITY = 70;


    public Led(string name, bool status, ColorOption color)
		:base(name, status)
	{
        colorOption = color;
	}

	public void ChangeColor(ColorOption colOption)
	{
		colorOption = colOption;
	}

	public void SetLightIntensity(int intensity)
	{
		LightIntensity = intensity;
    }

	



}
