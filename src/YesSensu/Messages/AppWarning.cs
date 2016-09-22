namespace YesSensu.Messages
{
    public class AppWarning : SensuBase
    {
        public AppWarning(string name) : base(name, Status.Warning)
        { }
    }
}