namespace YesSensu.Core.Messages
{
    public class Heartbeat : AppOk
    {
        public int Ttl { get; private set; }
        public Heartbeat(string appName, int period = 60) : base($"{appName}_heartbeat")
        {
            Ttl = period;
        }
    }
}