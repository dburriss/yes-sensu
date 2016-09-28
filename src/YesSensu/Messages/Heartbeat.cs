namespace YesSensu.Messages
{
    /// <summary>
    /// A message class representing a heartbeat from application `appName`
    /// </summary>
    public class Heartbeat : Ok
    {
        public int Ttl { get; private set; }

        /// <summary>
        /// Constructor for Heartbeat
        /// </summary>
        /// <param name="appName">The name of the application sending the hearbeat</param>
        /// <param name="period">The TTl. The period of time in seconds within which Sensu should expect another heartbeat</param>
        public Heartbeat(string appName, int period = 60) : base(appName, "heartbeat")
        {
            Ttl = period;
        }
    }
}