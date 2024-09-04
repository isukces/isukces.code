#nullable enable
using System;

namespace iSukces.Code.Db
{
    public class AutoNavigationAttribute : Attribute
    {
        public AutoNavigationAttribute(string name, Type type, string? inverse = null)
        {
            Name    = name;
            Type    = type;
            Inverse = inverse;
        }

        public AutoNavigationAttribute(Type type, string? inverse = null)
        {
            Type    = type;
            Inverse = inverse;
        }


        public string      Name                          { get; }
        public Type        Type                          { get; }
        public string      Inverse                       { get; }
        public InverseKind GenerateInverse               { get; set; }        
        public string      NavigationPropertyDescription { get; set; }
    }

    public enum InverseKind
    {
        None,
        Single,
        Collection
    }
}