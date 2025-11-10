using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

public class TwoLampsDevice()
{

    List<object> values = new List<object>();
    

    public void addLamp(Lamp lamp)
    {
        values.Add(lamp);
    }
    public void addEcoLamp(EcoLamp ecoLamp)
    {
        values.Add(ecoLamp);
    }

    public void TurnOnAllLamps()
    { 
        for (int i = 0; i < values.Count; i++)
        {
            if (values[i] is Lamp lamp)
            {
                lamp.TurnOn();
            }
            else if (values[i] is EcoLamp ecoLamp)
            {
                ecoLamp.TurnOn();
            }
        }
    }

    public void TurnOffAllLamps()
    { 
        for (int i = 0; i < values.Count; i++)
        {
            if (values[i] is Lamp lamp)
            {
                lamp.TurnOff();
            }
            else if (values[i] is EcoLamp ecoLamp)
            {
                ecoLamp.TurnOff();
            }
        }
    }

    public void SetLuminosityAllLamps(int percentage)
    { 
        for (int i = 0; i < values.Count; i++)
        {
            if (values[i] is Lamp lamp)
            {
                lamp.SetLuminosity(percentage);
            }
            else if (values[i] is EcoLamp ecoLamp)
            {
                ecoLamp.SetLuminosity(percentage);
            }
        }
    }

    public void turnofflampsatindex(int index)
    {
        if (index < 0 || index >= values.Count)
        {
            return;
        }
        if (values[index] is Lamp lamp)
        {
            lamp.TurnOff();
        }
        else if (values[index] is EcoLamp ecoLamp)
        {
            ecoLamp.TurnOff();
        }
    }
    
    public void turnonlampsatindex(int index)
    {
        if (index < 0 || index >= values.Count)
        {
            return;
        }
        if (values[index] is Lamp lamp)
        {
            lamp.TurnOn();
        }
        else if (values[index] is EcoLamp ecoLamp)
        {
            ecoLamp.TurnOn();
        }
    }

    public void setluminosityatindex(int index, int percentage)
    {
        if (index < 0 || index >= values.Count)
        {
            return;
        }
        if (values[index] is Lamp lamp)
        {
            lamp.SetLuminosity(percentage);
        }
        else if (values[index] is EcoLamp ecoLamp)
        {
            ecoLamp.SetLuminosity(percentage);
        }
    }

    public int getONLampsCount()
    {
        int count = 0;
        for (int i = 0; i < values.Count; i++)
        {
            if (values[i] is Lamp lamp && lamp.IsOn)
            {
                count++;
            }
            else if (values[i] is EcoLamp ecoLamp && ecoLamp.IsOn)
            {
                count++;
            }
        }
        return count;
    }

    public int getLampsCount()
    {
        return values.Count;
    }

    public object? getLampAtIndex(int index)
    {
        if (index < 0 || index >= values.Count)
        {
            return null;
        }
        return values[index];
    }

    public void RemoveLampAtIndex(int index)
    {
        if (index < 0 || index >= values.Count)
        {
            return;
        }
        values.RemoveAt(index);
    }


    public void ClearAllLamps()
    {
        values.Clear();
    }

    public void UpdateAllEcoLamps(DateTime now)
    {
        for (int i = 0; i < values.Count; i++)
        {
            if (values[i] is EcoLamp ecoLamp)
            {
                ecoLamp.Update(now);
            }
        }
    }

    public void RegisterPresenceAllEcoLamps()
    {
        for (int i = 0; i < values.Count; i++)
        {
            if (values[i] is EcoLamp ecoLamp)
            {
                ecoLamp.RegisterPresence();
            }
        }
    }

    public void ScheduleAllEcoLamps(DateTime? onTime, DateTime? offTime)
    {
        for (int i = 0; i < values.Count; i++)
        {
            if (values[i] is EcoLamp ecoLamp)
            {
                ecoLamp.Schedule(onTime, offTime);
            }
        }
    }

    










}
