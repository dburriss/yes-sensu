using YesSensu.Core;

namespace YesSensu.Messages
{
    /// <summary>
    /// A message class representing an error with subsystem `name` in application `appName`
    /// </summary>
    public class Error : SensuBase
    {
        /// <summary>
        /// Constructor for Error 
        /// </summary>
        /// <param name="appName">The name of the application sending the error message</param>
        /// <param name="name">Name of the sub-system that caused the error</param>
        public Error(string appName, string name) : base(appName, name, Status.Error)
        { }
    }
}