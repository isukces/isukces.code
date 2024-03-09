using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using iSukces.Code.Interfaces;
using JetBrains.Annotations;

namespace iSukces.Code.AutoCode;

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

        public string MakeShouldSerializeCondition(PropertyInfo pi)
        {
            var type       = pi.PropertyType;
            var isNullable = false;
            if (type.IsGenericType)
            {
                var type2 = type.GetGenericTypeDefinition();
                if (type2 == typeof(Nullable<>))
                {
                    isNullable = true;
                    type       = type.GenericTypeArguments[0];
                }
            }

            var template = GetTypeTemplate(type)?.Trim();
            if (isNullable)
            {
                var hasValueCondition = pi.Name + ".HasValue";
                if (string.IsNullOrEmpty(template))
                    return hasValueCondition;
                var customCondition = string.Format(template, pi.Name + ".Value");
                return hasValueCondition + " && " + customCondition;
            }

            if (!string.IsNullOrEmpty(template))
            {
                var typeName = Class.GetTypeName(pi.PropertyType);
                return string.Format(template, pi.Name, typeName.Declaration);
            }

            var message = $"Unable to get condition for property {pi.Name} declared in {pi.DeclaringType}, property type {pi.PropertyType} .";
            throw new Exception(message);

        }

        [CanBeNull]
        protected virtual string GetTypeTemplate(Type type)
        {
            if (Templates.TryGetValue(type, out var template))
                return template;
            var infoAttribute = type.GetTypeInfo().GetCustomAttribute<Auto.ShouldSerializeInfoAttribute>();
            return infoAttribute?.CodeTemplate;
        }


        protected override void GenerateInternal()
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
                var propertyName = i.Item1.Name;
                var condition    = i.Item2.Condition;
                if (string.IsNullOrEmpty(condition))
                    condition = MakeShouldSerializeCondition(i.Item1);
                else
                {
                    var typeName = Class.GetTypeName(i.Item1.PropertyType); 
                    condition = string.Format(condition, propertyName, typeName);
                }

                Class.AddMethod("ShouldSerialize" + propertyName, CsType.Bool)
                    .WithBodyAsExpression(condition);
            }
        }

        // ReSharper disable once MemberCanBePrivate.Global
        public static Dictionary<Type, string> Templates { get; }
    }
}