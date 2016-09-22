using YesSensu.Core;

namespace YesSensu.Messages
{
    public class Warning : SensuBase
    {
        public Warning(string appName, string name) : base(appName, name, Status.Warning)
        { }
    }
}