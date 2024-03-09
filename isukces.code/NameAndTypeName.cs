using System;

namespace iSukces.Code
{
    public struct NameAndTypeName
    {
        public NameAndTypeName(string propName, CsType propertyTypeName)
        {
            PropName         = propName;
            PropertyTypeName = propertyTypeName;
        }

        public string PropName         { get;  } 
        public CsType PropertyTypeName { get;  }
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