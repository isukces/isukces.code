#region using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using isukces.code.interfaces;

#endregion

namespace isukces.code.AutoCode
{
    internal partial class Generators
    {
        #region Nested

        public class BuilderGenerator : SingleClassGenerator, IAutoCodeGenerator
        {
            #region Static Methods

            private static List<string> GetConstructorArgs(ConstructorInfo constructor,
                IReadOnlyList<PropertyInfo> properties)
            {
                var args = new List<string>();
                var dictionary = properties.ToDictionary(a => a.Name, a => a, StringComparer.OrdinalIgnoreCase);
                var constructorParameters = constructor.GetParameters();
                foreach (var parameter in constructorParameters)
                {
                    PropertyInfo propertyInfo;
                    if (!dictionary.TryGetValue(parameter.Name, out propertyInfo))
                        return null;
                    args.Add(propertyInfo.Name);
                }
                return args;
            }

            private static HashSet<string> ToHashSet(List<string> list)
            {
                if (list == null) return new HashSet<string>();
                var r = new HashSet<string>();
                foreach (var i in list)
                    r.Add(i);
                return r;
            }

            #endregion

            #region Instance Methods

            public void Generate(Type type, IAutoCodeGeneratorContext context)
            {
                Setup(type, context);
                _attribute = type.GetCustomAttribute<Auto.BuilderAttribute>();
                GenerateInternal();
            }

            private void GenerateInternal()
            {
                if (_attribute == null)
                    return;
                {
                    var cv = Class.GetOrCreateNested(GetClassName());
                    var properties = GetInstanceProperties();
                    foreach (var i in properties)
                    {
                        var prop = cv.AddProperty(i.Name, i.PropertyType);
                        prop.MakeAutoImplementIfPossible = true;

                        var m = cv.AddMethod("With" + i.Name, "Builder", "");
                        m.AddParam("value", cv.TypeName(i.PropertyType), "new value");
                        m.Body = $"this.{i.Name} = value;\r\nreturn this;";
                    }
                    {
                        var constructorArgs = GetConstructorArgs(properties);
                        var usedProperties = ToHashSet(constructorArgs);
                        var cw = new CodeFormatter();
                        cw.Writeln("// ReSharper disable once UseObjectOrCollectionInitializer");
                        cw.Writeln("var result = new {0}({1});", Type.Name, string.Join(", ", constructorArgs));
                        foreach (var i in properties)
                            if (!usedProperties.Contains(i.Name))
                                cw.Writeln("result.{0} = {0};", i.Name);
                        cw.Writeln("return result;");
                        var m = cv.AddMethod("Build", Type.Name, "");
                        m.Body = cw.Text;
                    }
                }
            }

            private string GetClassName()
            {
                var name = _attribute?.BuilderClassName?.Trim();
                return string.IsNullOrEmpty(name) ? "Builder" : name;
            }


            private List<string> GetConstructorArgs(IReadOnlyList<PropertyInfo> properties)
            {
                var constructors = Type
                    .GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly)
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
                return Type.GetProperties(allInstanceProperties);
            }

            #endregion

            #region Fields

            private Auto.BuilderAttribute _attribute;

            #endregion

            // private readonly AutoCodeGeneratorConfiguration _configuration;
        }

        #endregion
    }
}