using System;

namespace iSukces.Code
{
    public struct NameAndTypeName
    {
        public NameAndTypeName(string propName, string propertyTypeName)
        {
            PropName         = propName;
            PropertyTypeName = propertyTypeName;
        }

        public string PropName         { get;  } 
        public string PropertyTypeName { get;  }
    }
    
    public struct NameAndType
    {
        public NameAndType(string name, Type type)
        {
            Name = name;
            Type = type;
        }

        public string Name { get; }
        public Type   Type { get; }
    }
}