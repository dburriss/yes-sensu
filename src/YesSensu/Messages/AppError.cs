namespace YesSensu.Core.Messages
{
    public class AppError : SensuBase
    {
        public AppError(string name) : base(name, Status.Error)
        { }
    }
}