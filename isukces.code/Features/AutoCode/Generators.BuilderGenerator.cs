﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using iSukces.Code.Interfaces;

namespace iSukces.Code.AutoCode
{
    public partial class Generators
    {
        public class BuilderGenerator : SingleClassGenerator<Auto.BuilderAttribute>, IAutoCodeGenerator
        {
            private static List<string> GetConstructorArgs(ConstructorInfo constructor,
                IReadOnlyList<PropertyInfo> properties)
            {
                var args = new List<string>();
                var dictionary = properties.ToDictionary(a => a.Name, a => a, StringComparer.OrdinalIgnoreCase);
                var constructorParameters = constructor.GetParameters();
                foreach (var parameter in constructorParameters)
                {
                    if (!dictionary.TryGetValue(parameter.Name, out var propertyInfo))
                        return null;
                    args.Add(propertyInfo.Name);
                }
                return args;
            }

           
            
            protected override void GenerateInternal()
            {
                {
                    var cv = Class.GetOrCreateNested(GetClassName());
                    var properties = GetInstanceProperties();
                    foreach (var i in properties)
                    {
                        var prop = cv.AddProperty(i.Name, i.PropertyType);
                        prop.MakeAutoImplementIfPossible = true;

                        var m = cv.AddMethod("With" + i.Name, "Builder", "");
                        m.AddParam("value", cv.GetTypeName(i.PropertyType), "new value");
                        m.Body = $"this.{i.Name} = value;\r\nreturn this;";
                    }
                    {
                        var constructorArgs = GetConstructorArgs(properties);
                        var usedProperties = constructorArgs.ToHashSet();
                        var cw = new CsCodeWriter();
                        //cw.WriteLine("// ReSharper disable UseObjectOrCollectionInitializer");
                        // cw.WriteLine("// ReSharper disable MemberCanBeMadeStatic.Local");
                        cw.WriteLine("var result = new {0}({1});", Type.Name, string.Join(", ", constructorArgs));
                        foreach (var i in properties)
                            if (!usedProperties.Contains(i.Name))
                                cw.WriteLine("result.{0} = {0};", i.Name);
                        cw.WriteLine("return result;");
                        var m = cv.AddMethod("Build", Type.Name, "");
                        m.Body = cw.Code;
                    }
                }
            }

            private string GetClassName()
            {
                var name = Attribute?.BuilderClassName?.Trim();
                return string.IsNullOrEmpty(name) ? "Builder" : name;
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

            private IReadOnlyList<PropertyInfo> GetInstanceProperties()
            {
                const BindingFlags allInstanceProperties =
                    BindingFlags.Instance
                    | BindingFlags.Public
                    | BindingFlags.NonPublic;
                return Type
#if COREFX
                    .GetTypeInfo()
#endif
                    .GetProperties(allInstanceProperties);
            }

          
        }
    }
}