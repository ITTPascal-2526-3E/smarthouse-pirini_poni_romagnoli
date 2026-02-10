using BlaisePascal.SmartHouse.Domain.Abstraction;

namespace BlaisePascal.SmartHouse.Domain.security
{
    public abstract class SecurityDevice : Device
    {
        public bool IsArmed { get; protected set; }

        protected SecurityDevice(string name, bool status) : base(name, status)
        {
            IsArmed = true; // Default to armed for security
        }


        public event Action<string, string>? OnAlarm;

        public abstract void TriggerAlarm();

        protected void RaiseAlarm(string message)
        {
             // Manual check: only call the subscribers if some exist (not null)
             if (OnAlarm != null)
             {
                 OnAlarm(Name.Value, message);
             }
        }

        public virtual void Arm()
        {
            IsArmed = true;
            Touch();
        }

        public virtual void Disarm()
        {
            IsArmed = false;
            Touch();
        }

        public override string ToString()
        {
            return $"{base.ToString()}, Secured: {(IsArmed ? "YES" : "NO")}";
        }
    }
}
