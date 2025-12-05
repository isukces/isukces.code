using System;
using System.Collections.Generic;
using System.Reflection;

namespace iSukces.Code.Translations;

public static class ReflectionProxy
{
    extension(Type type)
    {
        public IReadOnlyList<MethodInfo> GetMethodsX(BindingFlags flags)
        {
            if (GetMethods is not null)
                return GetMethods(type, flags);
            return type.GetMethods(flags);
        }

        public IReadOnlyList<PropertyInfo> GetPropertiesX(BindingFlags flags)
        {
            if (GetProperties is not null)
                return GetProperties(type, flags);
            return type.GetProperties(flags);
        }
    }
    
    public static Func<Type, BindingFlags, IReadOnlyList<MethodInfo>>? GetMethods     { get; set; } 
    
    public static Func<Type, BindingFlags, IReadOnlyList<PropertyInfo>>? GetProperties { get; set; } 
}
