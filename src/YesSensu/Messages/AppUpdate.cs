namespace YesSensu.Core.Messages
{
    public class AppUpdate : SensuMeta
    {
        public string Source { get; private set; }
        public AppUpdate(string appName, string key, Status status) : base(key, status)
        {
            Source = appName;
        }
    }
}
