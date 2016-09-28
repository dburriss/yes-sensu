using System;
using System.Collections.Generic;
using YesSensu.Core;

namespace YesSensu.Messages
{
    /// <summary>
    /// Abstract base class for Sensu messages. It is not required but provides some standard properties as well as methods for adding meta data.
    /// </summary>
    public abstract class SensuBase : IHaveMeta
    {
        public string Name { get; private set; }
        public string Output { get; set; }
        public string Source { get; protected set; }
        public Status Status { get; private set; }

        private readonly Dictionary<string, object> _meta = new Dictionary<string, object>();
        public IDictionary<string, object> Meta => _meta;

        /// <summary>
        /// Constructor for SensuBase
        /// </summary>
        /// <param name="appName">Name of application sending monitoring</param>
        /// <param name="name">Name of subsystem being reported on.</param>
        /// <param name="status">Status of the subsystem</param>
        protected SensuBase(string appName, string name, Status status)
        {
            if(string.IsNullOrEmpty(appName))
                throw new ArgumentNullException(nameof(appName));

            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            Source = appName;
            Name = name;
            Status = status;
            Output = "";
        }

        /// <summary>
        /// Adds a value to the Meta data
        /// </summary>
        /// <param name="name">Key in the meta data</param>
        /// <param name="data">Data added to the meta data</param>
        public void AddMeta(string name, object data)
        {
            _meta[name] = data;
        }

    }
}