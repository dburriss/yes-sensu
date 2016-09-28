using System;
using System.Collections.Generic;
using YesSensu.Core;

namespace YesSensu.Messages
{
    public abstract class SensuBase : IHaveMeta
    {
        public string Name { get; private set; }
        public string Output { get; set; }
        public string Source { get; protected set; }
        public Status Status { get; private set; }

        private readonly Dictionary<string, object> _meta = new Dictionary<string, object>();
        public IDictionary<string, object> Meta => _meta;


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

        public void AddMeta(string name, object data)
        {
            _meta[name] = data;
        }

    }
}