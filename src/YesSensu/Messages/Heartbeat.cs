namespace YesSensu.Messages
{
    public class Heartbeat : Ok
    {
        public int Ttl { get; private set; }
        public Heartbeat(string appName, int period = 60) : base(appName, "heartbeat")
        {
            Ttl = period;
        }
    }
}