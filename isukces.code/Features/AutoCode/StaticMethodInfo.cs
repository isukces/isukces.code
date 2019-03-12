using System;

namespace isukces.code
{
    public class StaticMethodInfo
    {
        public StaticMethodInfo(Type ownerType, string methodName)
        {
            OwnerType = ownerType;
            MethodName = methodName;
        }

        public Type   OwnerType  { get; }
        public string MethodName { get; }
        public bool   IsEmpty    => OwnerType == null || string.IsNullOrEmpty(MethodName);
    }
}