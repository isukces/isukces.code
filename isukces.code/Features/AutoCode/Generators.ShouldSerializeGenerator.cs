using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using isukces.code.CodeWrite;
using isukces.code.interfaces;

namespace isukces.code.AutoCode
{
    public partial class Generators
    {
        public class ShouldSerializeGenerator : SingleClassGenerator, IAutoCodeGenerator
        {
            static ShouldSerializeGenerator()
            {
                Templates = new Dictionary<Type, string>
                {
                    [typeof(int)]    = "{0} != 0",
                    [typeof(bool)]   = "{0}",
                    [typeof(Guid)]   = "!Guid.Empty.Equals({0})",
                    [typeof(string)] = "!string.IsNullOrEmpty({0})"
                };
            }

            private static string GetTypeTemplate(Type type)
            {
                if (Templates.TryGetValue(type, out var template))
                    return template;

                var infoAttribute = type.GetTypeInfo().GetCustomAttribute<Auto.ShouldSerializeInfoAttribute>();
                if (infoAttribute != null)
                    return infoAttribute.CodeTemplate;
                throw new Exception("Unable to get condition for " + type);
            }

            public static string MakeShouldSerializeCondition(PropertyInfo pi)
            {
                var type       = pi.PropertyType;
                var isNullable = false;
                if (type
#if COREFX
                    .GetTypeInfo()
#endif
                    .IsGenericType)
                {
                    var type2 = type.GetGenericTypeDefinition();
                    if (type2 == typeof(Nullable<>))
                    {
                        isNullable = true;
                        type       = type.GenericTypeArguments[0];
                    }
                }

                var template = GetTypeTemplate(type);
                if (isNullable)
                    return string.Format("{0} != null && ", pi.Name)
                           + string.Format(template, pi.Name + ".Value");
                return string.Format(template, pi.Name);
            }

            public void Generate(Type type, IAutoCodeGeneratorContext context)
            {
                Setup(type, context);
                GenerateInternal();
            }

            private void GenerateInternal()
            {
                var properties = Type
#if COREFX
                    .GetTypeInfo()
#endif
                    .GetProperties(BindingFlags.Instance | BindingFlags.Public);
                if (properties.Length == 0) return;
                var list = (from i in properties
                    let at = i.GetCustomAttribute<Auto.ShouldSerializeAttribute>(false)
                    where at != null
                    select Tuple.Create(i, at)).ToList();
                if (list.Count == 0)
                    return;

                foreach (var i in list)
                {
                    if (i.Item1.DeclaringType != Type)
                        continue;
                    var m         = Class.AddMethod("ShouldSerialize" + i.Item1.Name, "bool", null);
                    var writer    = new CsCodeWriter();
                    var condition = i.Item2.Condition;
                    if (string.IsNullOrEmpty(condition))
                        condition = MakeShouldSerializeCondition(i.Item1);
                    writer.WriteLine("return {0};", condition);
                    m.Body = writer.Code;
                }
            }

            // ReSharper disable once MemberCanBePrivate.Global
            public static Dictionary<Type, string> Templates { get; }
 
        }
    }
}