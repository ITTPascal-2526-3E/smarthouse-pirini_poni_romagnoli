using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

public class LampsRow : Device
{
    private Guid DeviceId { get; } = Guid.NewGuid();

    // RIMOSSO: private string name... -> Ora usiamo la proprietà 'Name' ereditata da Device

    // Usiamo la lista di 'Lamp' per sfruttare il polimorfismo (EcoLamp è una Lamp)
    private List<Lamp> values = new List<Lamp>();

    // Costruttore: Inizializza il genitore Device
    // Impostiamo il nome di default e lo stato iniziale a false (spento)
    public LampsRow() : base("Unnamed LampsRow", false)
    {
    }

    public void AddLamp(Lamp lamp)
    {
        values.Add(lamp);
    }

    public void AddEcoLamp(EcoLamp ecoLamp)
    {
        values.Add(ecoLamp);
    }

    public void TurnOnAllLamps()
    {
        // Aggiorniamo lo stato della Fila stessa
        Status = true;
        LastmodifiedAtUtc = DateTime.Now;

        foreach (var lamp in values)
        {
            lamp.TurnOn();
        }
    }

    public void TurnOffAllLamps()
    {
        // Aggiorniamo lo stato della Fila stessa
        Status = false;
        LastmodifiedAtUtc = DateTime.Now;

        foreach (var lamp in values)
        {
            lamp.TurnOff();
        }
    }

    public void SetLuminosityAllLamps(int percentage)
    {
        // Se si imposta la luminosità, consideriamo la fila "accesa" se > 0
        if (percentage > 0 && !Status)
        {
            Status = true;
            LastmodifiedAtUtc = DateTime.Now;
        }

        foreach (var lamp in values)
        {
            lamp.SetLuminosity(percentage);
        }
    }

    public void TurnOffLampAtIndex(int index)
    {
        if (index < 0 || index >= values.Count) return;

        values[index].TurnOff();
        LastmodifiedAtUtc = DateTime.Now; // La fila è cambiata
    }

    public void TurnOnLampAtIndex(int index)
    {
        if (index < 0 || index >= values.Count) return;

        values[index].TurnOn();
        // Se accendiamo una lampada, la fila è tecnicamente "attiva"
        Status = true;
        LastmodifiedAtUtc = DateTime.Now;
    }

    public void SetLuminosityAtIndex(int index, int percentage)
    {
        if (index < 0 || index >= values.Count) return;

        values[index].SetLuminosity(percentage);
        LastmodifiedAtUtc = DateTime.Now;
    }

    public int GetONLampsCount()
    {
        int count = 0;
        foreach (var lamp in values)
        {
            if (lamp.IsOn) count++;
        }
        return count;
    }

    public int GetLampsCount()
    {
        return values.Count;
    }

    public Lamp? GetLampAtIndex(int index)
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
        LastmodifiedAtUtc = DateTime.Now;
    }


    public void ClearAllLamps()
    {
        values.Clear();
        Status = false; // Se non ci sono lampade, la fila è spenta
        LastmodifiedAtUtc = DateTime.Now;
    }

    // --- Metodi specifici EcoLamp ---

    public void UpdateAllEcoLamps(DateTime now)
    {
        foreach (var lamp in values)
        {
            if (lamp is EcoLamp ecoLamp)
            {
                ecoLamp.Update(now);
            }
        }
    }

    public void RegisterPresenceAllEcoLamps()
    {
        foreach (var lamp in values)
        {
            if (lamp is EcoLamp ecoLamp)
            {
                ecoLamp.RegisterPresence();
            }
        }
    }

    public void ScheduleAllEcoLamps(DateTime? onTime, DateTime? offTime)
    {
        foreach (var lamp in values)
        {
            if (lamp is EcoLamp ecoLamp)
            {
                ecoLamp.Schedule(onTime, offTime);
            }
        }
    }
}