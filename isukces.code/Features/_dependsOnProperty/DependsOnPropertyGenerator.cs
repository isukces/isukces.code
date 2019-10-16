using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using isukces.code.AutoCode;

namespace isukces.code._dependsOnProperty
{
    public class DependsOnPropertyGenerator : Generators.SingleClassGenerator
    {
        protected virtual string GetConstName(string propertyName)
        {
            return propertyName + "Dependent";
        }

        protected override void GenerateInternal()
        {
            Dictionary<string, List<string>> slaves = null;
            var props = Type
#if COREFX
                    .GetTypeInfo()
#endif
                .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (var property in props)
            {
                var atts = property.GetCustomAttributes<DependsOnPropertyAttribute>();
                foreach (var masterName in atts.SelectMany(q => q.PropertyNames))
                {
                    if (slaves is null)
                        slaves = new Dictionary<string, List<string>>();
                    if (!slaves.TryGetValue(masterName, out var list))
                        slaves[masterName] = list = new List<string>();
                    list.Add(property.Name);
                }
            }

            if (slaves is null)
                return;

            void ScanDeep(string key, ISet<string> scanned, List<string> sink)
            {
                if (!scanned.Add(key))
                    return;
                if (!slaves.TryGetValue(key, out var list)) return;
                sink.AddRange(list);
                foreach (var ii in list)
                    ScanDeep(ii, scanned, sink);
            }

            var added = new Dictionary<string, string>();
            foreach (var i in slaves)
            {
                var sink = new List<string>();
                ScanDeep(i.Key, new HashSet<string>(), sink);
                var value     = string.Join(",", sink.Distinct());
                var constName = GetConstName(i.Key);
                if (added.TryGetValue(value, out var previousConstName))
                    Class.AddConst(constName, "string", previousConstName);
                else
                {
                    Class.AddConstString(constName, value);
                    added[value] = constName;
                }
            }
        }
    }
}