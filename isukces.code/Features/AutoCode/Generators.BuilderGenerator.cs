using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using iSukces.Code.Interfaces;

namespace iSukces.Code.AutoCode;

public partial class Generators
{
    public class BuilderGenerator : SingleClassGenerator<Auto.BuilderAttribute>, IAutoCodeGenerator
    {
        private static List<string>? GetConstructorArgs(ConstructorInfo constructor,
            IReadOnlyList<PropertyInfo> properties)
        {
            var args                  = new List<string>();
            var dictionary            = properties.ToDictionary(a => a.Name, a => a, StringComparer.OrdinalIgnoreCase);
            var constructorParameters = constructor.GetParameters();
            foreach (var parameter in constructorParameters)
            {
                var parameterName = parameter.Name;
                Debug.Assert(parameterName != null, nameof(parameterName) + " != null");
                if (!dictionary.TryGetValue(parameterName, out var propertyInfo))
                    return null;
                args.Add(propertyInfo.Name);
            }

            return args;
        }


        protected override void GenerateInternal()
        {
            {
                var cv         = Class.GetOrCreateNested(GetClassName());
                var properties = GetInstanceProperties();
                foreach (var i in properties)
                {
                    var prop = cv.AddProperty(i.Name, i.PropertyType);
                    prop.MakeAutoImplementIfPossible = true;

                    var m = cv.AddMethod("With" + i.Name, (CsType)"Builder", "");
                    m.AddParam("value", cv.GetTypeName(i.PropertyType), "new value");
                    m.Body = $"this.{i.Name} = value;\r\nreturn this;";
                }

                {
                    var constructorArgs = GetConstructorArgs(properties);
                    var usedProperties  = constructorArgs.ToHashSet();
                    var cw              = new CsCodeWriter();
                    //cw.WriteLine("// ReSharper disable UseObjectOrCollectionInitializer");
                    // cw.WriteLine("// ReSharper disable MemberCanBeMadeStatic.Local");
                    var mm = constructorArgs.CommaJoin().New(Type.Name);
                    cw.WriteLine($"var result = {mm};");
                    foreach (var i in properties)
                        if (!usedProperties.Contains(i.Name))
                            cw.WriteLine("result.{0} = {0};", i.Name);
                    cw.WriteLine("return result;");
                    var m = cv.AddMethod("Build", (CsType)Type.Name);
                    m.Body = cw.Code;
                }
            }
        }

        private CsType GetClassName()
        {
            var name = Attribute?.BuilderClassName?.Trim();
            return new CsType(string.IsNullOrEmpty(name) ? "Builder" : name);
        }

        private List<string> GetConstructorArgs(IReadOnlyList<PropertyInfo> properties)
        {
            var constructors = Type
#if COREFX
                    .GetTypeInfo()
#endif
                .GetConstructors(GeneratorsHelper.AllVisibility | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                .OrderByDescending(q => q.GetParameters().Length)
                .ToArray();
            foreach (var i in constructors)
            {
                var list = GetConstructorArgs(i, properties);
                if (list != null)
                    return list;
            }

            return new List<string>();
        }

        private IReadOnlyList<PropertyInfo> GetInstanceProperties() => Type.GetInstanceProperties();
    }
}

