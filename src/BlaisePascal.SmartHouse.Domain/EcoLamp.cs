public class EcoLamp : Lamp
{
    // gestione presenza nella stanza
    private TimeSpan noPresenceTimeout = TimeSpan.FromMinutes(5); // dopo 5 minuti senza presenza -> abbassa
    private DateTime lastPresenceTime;
    private int presenceDimLuminosity = 30; // quanto tenere la luce se non c’è nessuno

    // conteggio ore 
    private DateTime? lastTurnOnTime;
    public TimeSpan TotalOnTime { get; private set; } = TimeSpan.Zero;

    // programmazione 
    public DateTime? ScheduledOn { get; private set; }//il punto interrogativo indica che può essere null
    public DateTime? ScheduledOff { get; private set; }//uso il private set per evitare che venga modificato dall'esterno

    public EcoLamp(int power, string color, string model, string brand, string energyClass)
        : base(power, color, model, brand, energyClass)//chiamo il costruttore della classe base
    {
        lastPresenceTime = DateTime.Now;
    }

    // registrazione presenza
    public void RegisterPresence()
    {
        lastPresenceTime = DateTime.Now;
        if (base.IsOn && LuminosityPercentage < 100)
        {
            // torna alla luminosità piena quando qualcuno entra
            SetLuminosity(100);
        }
    }

    // pianificazione accensione/spegnimento
    public void Schedule(DateTime? onTime, DateTime? offTime)
    {
        ScheduledOn = onTime;
        ScheduledOff = offTime;
    }

    // Da chiamare periodicamente
    public void Update(DateTime now)
    {
        // 1. accensione programmata
        if (ScheduledOn.HasValue && now >= ScheduledOn.Value)
        {
            TurnOn();
            ScheduledOn = null; // eseguita
        }

        // 2. spegnimento programmato
        if (ScheduledOff.HasValue && now >= ScheduledOff.Value)
        {
            TurnOff();
            ScheduledOff = null; // eseguita
        }

        // 3. gestione assenza persone
        if (base.IsOn)
        {
            if (now - lastPresenceTime >= noPresenceTimeout)
            {
                // nessuna presenza da troppo -> abbassa
                if (LuminosityPercentage > presenceDimLuminosity)
                    SetLuminosity(presenceDimLuminosity);
            }
        }
    }

    
    public override void TurnOn()//override significa che sto ridefinendo un metodo della classe base
    {
        // se era spenta segno l'ora di accensione
        if (!base.IsOn)
            lastTurnOnTime = DateTime.Now;

        base.TurnOn();
    }


    // conteggio ore accesa
    public override void TurnOff()//override significa che sto ridefinendo un metodo della classe base
    {
        // se stava andando aggiungo le ore
        if (base.IsOn && lastTurnOnTime.HasValue)
        {
            TotalOnTime += DateTime.Now - lastTurnOnTime.Value;
            lastTurnOnTime = null;
        }

        base.TurnOff();
    }

}
