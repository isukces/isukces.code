using System;

namespace isukces.code
{
    public class EmitTypeAttribute : Attribute
    {
        public EmitTypeAttribute(string ns, string typeName=null)
        {
            Namespace = ns;
            TypeName = typeName;
        }

        public string Namespace { get; set; }
        public string TypeName { get; set; }
    }
}