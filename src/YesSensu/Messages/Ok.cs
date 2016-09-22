using YesSensu.Core;

namespace YesSensu.Messages
{
    public class Ok : SensuBase
    {
        public Ok(string appName, string name) : base(appName, name, Status.Ok)
        {
        }
    }
}