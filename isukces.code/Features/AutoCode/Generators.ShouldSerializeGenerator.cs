#region using

using System;
using System.Linq;
using System.Reflection;
using isukces.code.CodeWrite;
using isukces.code.interfaces;

#endregion

namespace isukces.code.AutoCode
{
    internal partial class Generators
    {
        #region Nested

        internal class ShouldSerializeGenerator : SingleClassGenerator, IAutoCodeGenerator
        {
            #region Static Methods

            private static string MakeShouldSerializeCondition(PropertyInfo pi)
            {
                if (pi.PropertyType == typeof(int))
                    return string.Format("{0} != 0", pi.Name);
                if (pi.PropertyType == typeof(Guid?))
                    return string.Format("{0} != null && !Guid.Empty.Equals({0}.Value)", pi.Name);
                if (pi.PropertyType == typeof(Guid))
                    return string.Format("!Guid.Empty.Equals({0})", pi.Name);
                if (pi.PropertyType == typeof(string))
                    return string.Format("!string.IsNullOrEmpty({0})", pi.Name);
                throw new Exception("Unable to get condition for " + pi.PropertyType);
            }

            #endregion

            #region Instance Methods

            public void Generate(Type type, IAutoCodeGeneratorContext context)
            {
                Setup(type, context);
                GenerateInternal();
            }

            private void GenerateInternal()
            {
                var properties = Type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
                if (properties.Length == 0) return;
                var list = (from i in properties
                    let at = i.GetCustomAttribute<Auto.ShouldSerializeAttribute>(false)
                    where at != null
                    select Tuple.Create(i, at)).ToList();
                if (list.Count == 0)
                    return;

                foreach (var i in list)
                {
                    var m = Class.AddMethod("ShouldSerialize" + i.Item1.Name, "bool", null);
                    var writer = new CodeWriter();
                    var condition = i.Item2.Condition;
                    if (string.IsNullOrEmpty(condition))
                        condition = MakeShouldSerializeCondition(i.Item1);
                    writer.WriteLine("return {0};", condition);
                    m.Body = writer.Code;
                }
            }

            #endregion
        }

        #endregion
    }
}