using System;
using System.Reflection;

namespace isukces.code
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Enum | AttributeTargets.Struct)]
    public class EmitTypeAttribute : Attribute
    {
        public static bool IgnoreEmitTypeAttribute { get; set; }
        
        public EmitTypeAttribute(string ns, string typeName = null)
        {
            Namespace = ns;
            TypeName  = typeName;
        }

        public string Namespace { get; set; }
        public string TypeName  { get; set; }

        
        public static EmitTypeAttribute GetAttribute(Type t)
        {
            if (IgnoreEmitTypeAttribute)
                return null;
            var at = t
#if COREFX
                .GetTypeInfo()
#endif
                .GetCustomAttribute<EmitTypeAttribute>();
            return at;
        }
    }
}