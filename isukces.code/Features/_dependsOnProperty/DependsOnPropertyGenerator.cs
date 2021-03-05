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

            var names = new Dictionary<string, Info>();

            void ScanDeep(string key, HashSet<string> scanned, List<string> sink)
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
            if ((Flags & DependsOnPropertyGeneratorF.GetDependentProperties) != 0)
                MakeGetDependentProperties(names);
        }

        protected virtual string GetConstName(string propertyName) => propertyName + "Dependent";

        private void MakeGetDependentProperties(Dictionary<string, Info> names)
        {
            var wr = new CsCodeWriter();
            wr.Open("switch (propertyName)");
            foreach (var i in names.GroupBy(a => a.Value.ConstValue))
            {
                var properties = i.OrderBy(a => a.Key).ToArray();
                var left       = properties.Length;
                foreach (var ii in properties.OrderBy(a => a.Key))
                {
                    var caseTxt = $"case nameof({ii.Key}):";
                    if (--left == 0)
                        caseTxt += $" return {ii.Value.ConstName}; // {ii.Value.ConstValue}";
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

        public DependsOnPropertyGeneratorF Flags { get; set; }


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
    public enum DependsOnPropertyGeneratorF
    {
        None = 0,
        GetDependentProperties = 1
    }
}