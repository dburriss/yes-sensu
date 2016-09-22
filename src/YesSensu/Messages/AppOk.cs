namespace YesSensu.Messages
{
    public class AppOk : SensuBase
    {
        public AppOk(string appName) : base(appName, Status.Ok)
        {
        }
    }
}