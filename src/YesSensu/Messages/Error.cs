using YesSensu.Core;

namespace YesSensu.Messages
{
    public class Error : SensuBase
    {
        public Error(string appName, string name) : base(appName, name, Status.Error)
        { }
    }
}