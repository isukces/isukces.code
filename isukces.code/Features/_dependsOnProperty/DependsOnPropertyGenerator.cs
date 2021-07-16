using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using iSukces.Code.AutoCode;
using iSukces.Code.Interfaces;

namespace iSukces.Code
{
    public class DependsOnPropertyGenerator : Generators.SingleClassGenerator
    {
        protected virtual void CreateAdditionalCode(Dictionary<string, Info> names)
        {
        }

        protected override void GenerateInternal()
        {
            Dictionary<string, HashSet<string>>        slavesForMaster = null;
            Dictionary<string, DependsOnPropertyFlags> flags  = null;
            var props = Type
#if COREFX
                .GetTypeInfo()
#endif
                .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (var property in props)
            {
                var atts = property.GetCustomAttributes<DependsOnPropertyAttribute>().ToArray();
                if (atts.Length == 0)
                    continue;
                if (flags is null)
                    flags = new Dictionary<string, DependsOnPropertyFlags>();
                foreach (var att in atts)
                {
                    flags.TryGetValue(property.Name, out var flag);
                    flags[property.Name] = flag | att.Flags;
                }
                
                foreach (var masterName in atts.SelectMany(q => q.PropertyNames))
                {
                    if (slavesForMaster is null)
                        slavesForMaster = new Dictionary<string, HashSet<string>>();
                    if (!slavesForMaster.TryGetValue(masterName, out var list))
                        slavesForMaster[masterName] = list = new HashSet<string>();
                    list.Add(property.Name);
                }
            }

            if (slavesForMaster is null)
                return;

            var names = new Dictionary<string, Info>();

            void ScanDeep(string key, HashSet<string> scanned, List<string> sink)
            {
                if (!scanned.Add(key))
                    return;
                if (!slavesForMaster.TryGetValue(key, out var list)) return;
                sink.AddRange(list);
                foreach (var ii in list)
                    ScanDeep(ii, scanned, sink);
            }

            var added = new Dictionary<string, string>();
            foreach (var i in slavesForMaster)
            {
                var sink = new List<string>();
                ScanDeep(i.Key, new HashSet<string>(), sink);
                var value     = string.Join(",", sink.Distinct());
                var constName = GetConstName(i.Key);

                names[i.Key] = new Info(constName, value);
                if (added.TryGetValue(value, out var previousConstName))
                {
                    Class.AddConst(constName, "string", previousConstName);
                }
                else
                {
                    Class.AddConstString(constName, value);
                    added[value] = constName;
                }
            }

            CreateAdditionalCode(names);
            if ((Flags & DependsOnPropertyGeneratorFlags.GetDependentProperties) != 0)
                MakeGetDependentProperties(names, flags);
        }

        protected virtual string GetConstName(string propertyName) => propertyName + "Dependent";

        private void MakeGetDependentProperties(Dictionary<string, Info> names,
            Dictionary<string, DependsOnPropertyFlags> dependsOnPropertyFlagsMap)
        {
            var wr = new CsCodeWriter();
            wr.Open("switch (propertyName)");
            foreach (var i in names.GroupBy(a => a.Value.ConstValue))
            {
                var properties = i
                    .Where(pair =>
                    {
                        dependsOnPropertyFlagsMap.TryGetValue(i.Key, out var flag1);
                        return (flag1 & DependsOnPropertyFlags.ExcludeFromGetDependentPropertiesMethod) == 0;
                    })
                    .OrderBy(a => a.Key).ToArray();
                var left       = properties.Length;
                foreach (var ii in properties)
                {
                    string GetConstCode()
                    {
                        dependsOnPropertyFlagsMap.TryGetValue(i.Key, out var flag1);
                        if ((flag1 & DependsOnPropertyFlags.SkipCreatingConstants) == 0)
                            return ii.Value.ConstName;

                        // search other existing consts
                        var other = properties.Where(a
                                =>
                            {
                                dependsOnPropertyFlagsMap.TryGetValue(i.Key, out var flag2);
                                return (flag2 & DependsOnPropertyFlags.SkipCreatingConstants) == 0;
                            })
                            .ToArray();
                        if (other.Length > 0)
                            return other[0].Value.ConstName;
                        return ii.Value.ConstValue.CsEncode();
                    }
                    var caseTxt = $"case nameof({ii.Key}):";
                    if (--left == 0)
                    {
                        var value = GetConstCode();
                        caseTxt += $" return {value};";

                        if (!value.StartsWith("\"", StringComparison.Ordinal)) 
                            caseTxt += $" // {ii.Value.ConstValue}";
                    }
                    wr.WriteLine(caseTxt);
                }
            }

            wr.Close();
            wr.WriteLine("return null;");

            var myClass = Class;
            var at = CsAttribute.Make<MethodImplAttribute>(myClass)
                .WithArgumentCode(myClass.GetEnumValueCode(MethodImplOptions.AggressiveInlining));

            myClass.AddMethod(GetDependentPropertiesMethodName, "string")
                .WithVisibility(Visibilities.Private)
                .WithStatic()
                .WithBody(wr)
                .WithParameter(new CsMethodParameter("propertyName", "string"))
                .WithAttributeFromName(at);
        }


        public string GetDependentPropertiesMethodName { get; set; } = "XGetDependentProperties";

        public DependsOnPropertyGeneratorFlags Flags { get; set; }


        public Visibilities GetDependentPropertiesMethodVisibility { get; set; } = Visibilities.Private;

        protected struct Info
        {
            public Info(string constName, string constValue)
            {
                ConstName  = constName;
                ConstValue = constValue;
            }

            public string ConstName  { get; }
            public string ConstValue { get; }
        }
    }

    [Flags]
    public enum DependsOnPropertyGeneratorFlags
    {
        None = 0,
        /// <summary>
        /// Create GetDependentProperties method.
        /// See also <see cref="DependsOnPropertyFlags">DependsOnPropertyFlags</see>.
        /// <see cref="DependsOnPropertyFlags.ExcludeFromGetDependentPropertiesMethod">ExcludeFromGetDependentPropertiesMethod</see>
        /// </summary>
        GetDependentProperties = 1
    }
}