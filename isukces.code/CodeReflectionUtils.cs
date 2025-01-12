using System;
using System.Collections.Generic;
using System.Reflection;

namespace iSukces.Code;

public static class CodeReflectionUtils
{
    public static  IReadOnlyList<PropertyInfo> GetInstanceProperties(this Type type)
    {
        const BindingFlags allInstanceProperties =
            BindingFlags.Instance
            | BindingFlags.Public
            | BindingFlags.NonPublic;
        return type
#if COREFX
                    .GetTypeInfo()
#endif
            .GetProperties(allInstanceProperties);
    }

         
}
