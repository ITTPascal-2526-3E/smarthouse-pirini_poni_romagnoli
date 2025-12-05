using BlaisePascal.SmartHouse.Domain.@enum;
using BlaisePascal.SmartHouse.Domain.illumination;
using System;

namespace BlaisePascal.SmartHouse.Domain
{
    // Represents a smart eco-friendly lamp with presence detection and scheduling
    public class EcoLamp : Lamp
    {
        // Stores the last time presence was detected in the room
        private DateTime lastPresenceTime;

        // Stores the moment when the lamp was last turned on (null when OFF)
        private DateTime? lastTurnOnTime;

        // Total accumulated time the lamp has been ON
        public TimeSpan TotalOnTime { get; private set; } = TimeSpan.Zero;

        // Scheduled ON time (null means no ON schedule set)
        public DateTime? ScheduledOn { get; private set; }

        // Scheduled OFF time (null means no OFF schedule set)
        public DateTime? ScheduledOff { get; private set; }

        // Target brightness percentage when no presence is detected
        private const int PresenceDimLuminosity = 30;

        // Minutes of no presence before dimming the light
        private const int PresenceTimeoutMinutes = 5;

        // Time span representing the no-presence timeout
        private readonly TimeSpan noPresenceTimeout = TimeSpan.FromMinutes(PresenceTimeoutMinutes);

        // Constructor calls the base lamp constructor and initializes presence tracking
        public EcoLamp(int power, ColorOption color, string model, string brand, EnergyClass energyClass, string name)
            : base(power, color, model, brand, energyClass, name)
        {
            lastPresenceTime = DateTime.UtcNow;
        }

        // Registers presence in the room and restores full brightness if the lamp is dimmed
        public void RegisterPresence()
        {
            lastPresenceTime = DateTime.UtcNow;

            // if the lamp is ON but dimmed(oscurato) due to no presence, restore full brightness
            if (IsOn && LuminosityPercentage < MaxLuminosity)
            {
                SetLuminosity(MaxLuminosity);
            }
        }

        // Sets ON/OFF scheduling times for the eco lamp
        public void Schedule(DateTime? onTime, DateTime? offTime)
        {
            ScheduledOn = onTime;   // null means no ON schedule
            ScheduledOff = offTime; // null means no OFF schedule
            Touch();
        }

        // Periodic update tick to execute schedules and presence-based dimming
        public void Update(DateTime now)
        {
            // Execute ON schedule when the scheduled time is reached
            if (ScheduledOn.HasValue && now >= ScheduledOn.Value)
            {
                TurnOn();
                ScheduledOn = null;
            }

            // Execute OFF schedule when the scheduled time is reached
            if (ScheduledOff.HasValue && now >= ScheduledOff.Value)
            {
                TurnOff();
                ScheduledOff = null;
            }

            // If the lamp is ON, check whether presence has been missing for too long
            if (IsOn)
            {
                if (now - lastPresenceTime >= noPresenceTimeout)
                {
                    // No presence for too long -> dim down to the target percentage
                    if (LuminosityPercentage > PresenceDimLuminosity)
                    {
                        SetLuminosity(PresenceDimLuminosity);
                    }
                }
            }
        }

        // Turns the lamp ON, records the ON time and updates last modified timestamp
        public override void TurnOn()
        {
            // If it was OFF, record the moment it gets turned ON
            if (!IsOn)
            {
                lastTurnOnTime = DateTime.UtcNow;
            }

            base.TurnOn();
            // base.TurnOn already calls Touch via the overridden method in Lamp
        }

        // Turns the lamp OFF, accumulates ON time and updates last modified timestamp
        public override void TurnOff()
        {
            // If it was ON, accumulate elapsed time since lastTurnOnTime
            if (IsOn && lastTurnOnTime.HasValue)
            {
                TotalOnTime += DateTime.UtcNow - lastTurnOnTime.Value;
                lastTurnOnTime = null; // Reset: not ON anymore
            }

            base.TurnOff();
            // base.TurnOff already calls Touch via the overridden method in Lamp
        }
    }
}
