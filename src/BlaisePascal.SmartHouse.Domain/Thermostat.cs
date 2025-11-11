using System;

public class Thermostat
{
    // Thermostat modes
    public enum ModeOption { Heating, Cooling, Off }
    public int CurrentTemperature { get; private set; }
    public ModeOption Mode { get; private set; }
    public int TargetTemperature { get; private set; }
    List<object> values = new List<object>();

    private Guid DeviceId { get; } = Guid.NewGuid();
    private string name { get; set; } = "Unnamed Thermostat";


    public Thermostat(int currtemp, ModeOption mod, int targtemp){

        CurrentTemperature = currtemp;
        Mode = mod;
        TargetTemperature = targtemp;

    }

    public void updateCurrentTemp(int temp)
    {
        CurrentTemperature = temp;

    }

    public void SetMode(ModeOption mode)
    {
        Mode = mode;
        for (int i = 0; i < values.Count; i++)
        {
            // 1. Eseguo il cast sicuro a HeatPump
            if (values[i] is HeatPump heatPump)
            {
                // 2. Converto l'enum del Thermostat in quello dell'HeatPump
                //    Assumendo che i nomi delle opzioni corrispondano
                HeatPump.ModeOption hpMode = (HeatPump.ModeOption)Enum.Parse(
                    typeof(HeatPump.ModeOption), mode.ToString());

                // 3. Chiamo il metodo sull'istanza 'heatPump'
                heatPump.SetMode(hpMode);
            }
        }
    }

    public void SetTargetTemperature(int temperature)
    {
        TargetTemperature = temperature;
        for (int i = 0; i < values.Count; i++)
        {
            // 1. Eseguo il cast sicuro a HeatPump
            if (values[i] is HeatPump heatPump)
            {
                // 2. Chiamo il metodo sull'istanza 'heatPump' con il nome corretto
                heatPump.SetTemperature(temperature);
            }
        }
    }
}
