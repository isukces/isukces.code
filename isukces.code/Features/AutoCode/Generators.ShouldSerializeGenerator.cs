using System;
using System.Linq;
using System.Reflection;
using isukces.code.CodeWrite;
using isukces.code.interfaces;

namespace isukces.code.AutoCode
{
    public partial class Generators
    {
        internal class ShouldSerializeGenerator : SingleClassGenerator, IAutoCodeGenerator
        {
            private static string MakeShouldSerializeCondition(PropertyInfo pi)
            {
                var type = pi.PropertyType;
                if (type == typeof(int))
                    return string.Format("{0} != 0", pi.Name);
                if (type == typeof(bool))
                    return string.Format("{0}", pi.Name);
                if (type == typeof(Guid?))
                    return string.Format("{0} != null && !Guid.Empty.Equals({0}.Value)", pi.Name);
                if (type == typeof(Guid))
                    return string.Format("!Guid.Empty.Equals({0})", pi.Name);
                if (type == typeof(string))
                    return string.Format("!string.IsNullOrEmpty({0})", pi.Name);
                if (type
#if COREFX
                    .GetTypeInfo()
#endif
                    
                    .IsGenericType)
                {
                    var type2 = type.GetGenericTypeDefinition();
                    if (type2 == typeof(Nullable<>))
                        return string.Format("{0} != null", pi.Name);
                }
                throw new Exception("Unable to get condition for " + pi.PropertyType);
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
                    var m = Class.AddMethod("ShouldSerialize" + i.Item1.Name, "bool", null);
                    var writer = new CodeWriter();
                    var condition = i.Item2.Condition;
                    if (string.IsNullOrEmpty(condition))
                        condition = MakeShouldSerializeCondition(i.Item1);
                    writer.WriteLine("return {0};", condition);
                    m.Body = writer.Code;
                }
            }
        }
    }
}