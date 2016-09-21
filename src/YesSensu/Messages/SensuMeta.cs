using System.Collections.Generic;

namespace YesSensu.Core.Messages
{
    public class SensuMeta : SensuBase
    {
        private readonly Dictionary<string, object> _meta = new Dictionary<string, object>();
        public IDictionary<string, object> Meta => _meta;

        public SensuMeta(string name, Status status) : base(name, status)
        { }

        public void AddMeta(string name, object data)
        {
            _meta.Add(name, data);
        }
    }
}