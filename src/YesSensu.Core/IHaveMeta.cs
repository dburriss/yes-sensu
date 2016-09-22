using System.Collections.Generic;

namespace YesSensu.Core
{
    public interface IHaveMeta
    {
        IDictionary<string, object> Meta { get; }
        void AddMeta(string name, object data);
    }
}