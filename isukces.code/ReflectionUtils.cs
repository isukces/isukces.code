using System;
using System.Reflection;

namespace iSukces.Code;

public static class ReflectionUtils
{
    public static PropertyInfo[] GetInstanceProperties(this Type type)
    {
        const BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
        return type.GetProperties(bindingFlags);
    }

         
}