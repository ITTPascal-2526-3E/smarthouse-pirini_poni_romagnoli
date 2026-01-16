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

        public abstract void TriggerAlarm();

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
