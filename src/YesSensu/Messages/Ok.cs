using YesSensu.Core;

namespace YesSensu.Messages
{
    /// <summary>
    /// A message class informing that subsystem `name` in application `appName` is ok
    /// </summary>
    public class Ok : SensuBase
    {
        /// <summary>
        /// Constructor for Error 
        /// </summary>
        /// <param name="appName">The name of the application sending the ok message</param>
        /// <param name="name">Name of the sub-system that is ok</param>
        public Ok(string appName, string name) : base(appName, name, Status.Ok)
        {
        }
    }
}