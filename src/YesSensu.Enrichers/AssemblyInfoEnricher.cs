using System;
using System.Linq;
using System.Reflection;
using YesSensu.Core;

namespace YesSensu.Enrichers
{
    /// <summary>
    /// I pull out Assembly attributes and add them to the meta data of a message
    /// </summary>
    public class AssemblyInfoEnricher : ISensuEnricher
    {
        private readonly Assembly _assembly;
        private readonly string _prefix;

        public AssemblyInfoEnricher()
        {
            _prefix = "";
            _assembly = Assembly.GetEntryAssembly();
        }

        public AssemblyInfoEnricher(Assembly assembly, string prefix = "")
        {
            _assembly = assembly;
            _prefix = prefix;
        }

        public void Enrich(IHaveMeta obj)
        {

            var attributes = _assembly.CustomAttributes.Where(x => x.AttributeType.Name.StartsWith("Assembly"));
            foreach (var attributeData in attributes)
            {
                var key = Key(attributeData);
                var value = Value(attributeData);
                obj.AddMeta(key, value);
            }

        }


        private object Value(CustomAttributeData t)
        {
            if (!t.ConstructorArguments.Any())
                return null;
            return t.ConstructorArguments.First().Value;
        }

        private string Key(CustomAttributeData t)
        {
            var key = t.AttributeType.Name.Replace("Assembly", "").Replace("Attribute", "");
            return _prefix + key;
        }
    }
}
