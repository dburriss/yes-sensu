using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using YesSensu.Core;

namespace YesSensu.Enrichers
{
    public class StaticEnricher : ISensuEnricher
    {
        private readonly IDictionary<string, object> _metaDataCache;
        private readonly bool _overrideDefault;
        private readonly IList<string> _keys;

        public StaticEnricher(IDictionary<string, object> metaData, bool overrideDefault = false)
        {
            if(metaData == null)
                throw new ArgumentNullException(nameof(metaData));

            _keys = metaData.Keys.ToList();
            _metaDataCache = metaData;
            _overrideDefault = overrideDefault;
        }
        public void Enrich(IHaveMeta obj)
        {
            if (obj == null)
                return;

            foreach (var key in _keys)
            {
                if(!obj.Meta.ContainsKey(key) || _overrideDefault)
                    obj.AddMeta(key, _metaDataCache[key]);
            }
        }
    }
}