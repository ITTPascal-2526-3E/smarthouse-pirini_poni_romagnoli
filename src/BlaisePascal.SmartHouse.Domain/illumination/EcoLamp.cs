using BlaisePascal.SmartHouse.Domain.illumination;
using System;

public class EcoLamp : Lamp
{
    //  Presence handling in the room 
    // After 5 minutes without detected presence, dim the light
    private DateTime lastPresenceTime;
    // Moment when the lamp was last turned on (nullable: it's null when currently OFF or not started yet)
    private DateTime? lastTurnOnTime;
    // Total accumulated time the lamp has been ON
    public TimeSpan TotalOnTime { get; private set; } = TimeSpan.Zero;

    //  Simple scheduling (ON/OFF) 
    // Nullable means: "no schedule set" (absence of a value)
    public DateTime? ScheduledOn { get; private set; }
    // Private setter prevents external code from changing it directly
    public DateTime? ScheduledOff { get; private set; }

    // Target brightness percentage when no one is around
    private const int PRESENCE_DIM_LUMINOSITY = 30;
    // Max luminosity percentage is a const
    private const int MAX_LUMINOSITY_PERCENTAGE = 100;
    // Minutes  
    private const int MINUTES = 5;
    // After 5 minutes without detected presence, dim the light
    private TimeSpan noPresenceTimeout = TimeSpan.FromMinutes(MINUTES);

    public EcoLamp(int power, ColorOption color, string model, string brand, EnergyClass energyClass, string nm)
        : base(power, color, model, brand, energyClass, nm) // Call base-class constructor
    {
        lastPresenceTime = DateTime.Now;
    }

    //  Presence registration
    public void RegisterPresence()
    {
        lastPresenceTime = DateTime.Now;

        // If the lamp is ON and currently dimmed, restore full brightness when presence is detected
        // base.IsOn ora legge correttamente lo Status della classe Device
        if (base.IsOn && LuminosityPercentage < MAX_LUMINOSITY_PERCENTAGE)
        {
            SetLuminosity(MAX_LUMINOSITY_PERCENTAGE);
        }
    }

    //  Set ON/OFF scheduling (nullable parameters mean "optional") 
    public void Schedule(DateTime? onTime, DateTime? offTime)
    {
        ScheduledOn = onTime;  // null => no ON schedule
        ScheduledOff = offTime; // null => no OFF schedule
    }

    //  Periodic update tick (call this from a timer/loop with the current time) 
    public void Update(DateTime now)
    {
        // 1) Execute ON schedule
        if (ScheduledOn.HasValue && now >= ScheduledOn.Value)
        {
            TurnOn();
            ScheduledOn = null;
        }

        // 2) Execute OFF schedule
        if (ScheduledOff.HasValue && now >= ScheduledOff.Value)
        {
            TurnOff();
            ScheduledOff = null;
        }

        // 3) Dim if no presence for a while
        if (base.IsOn)
        {
            if (now - lastPresenceTime >= noPresenceTimeout)
            {
                // No presence for too long -> dim down to the target percentage
                if (LuminosityPercentage > PRESENCE_DIM_LUMINOSITY)
                    SetLuminosity(PRESENCE_DIM_LUMINOSITY);
            }
        }
    }


    public override void TurnOn() // "override" = redefine a virtual method from the base class
    {
        // If it was OFF, record the moment it gets turned ON
        if (!base.IsOn)
            lastTurnOnTime = DateTime.Now;

        base.TurnOn(); // Questo aggiornerà anche LastmodifiedAtUtc in Device
    }

    //  Override: specialize base behavior to accumulate uptime on shutdown ---
    public override void TurnOff() // "override" = redefine a virtual method from the base class
    {
        // If it was ON, accumulate elapsed time since lastTurnOnTime
        if (base.IsOn && lastTurnOnTime.HasValue)
        {
            TotalOnTime += DateTime.Now - lastTurnOnTime.Value;
            lastTurnOnTime = null; // Reset: not ON anymore
        }

        base.TurnOff(); // Questo aggiornerà anche LastmodifiedAtUtc in Device
    }
}