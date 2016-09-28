using YesSensu.Core;

namespace YesSensu.Messages
{
    /// <summary>
    /// A message class representing an warning with subsystem `name` in application `appName`
    /// </summary>
    public class Warning : SensuBase
    {
        /// <summary>
        /// Constructor for Warning 
        /// </summary>
        /// <param name="appName">The name of the application sending the warning message</param>
        /// <param name="name">Name of the sub-system that caused the warning</param>
        public Warning(string appName, string name) : base(appName, name, Status.Warning)
        { }
    }
}