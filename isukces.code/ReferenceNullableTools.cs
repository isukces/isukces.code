using System.Linq;
using System.Reflection;

namespace iSukces.Code;

public static class ReferenceNullableTools
{
    public static bool IsReferenceTypeNullable(ParameterInfo parameter)
    {
        if (parameter.ParameterType.IsValueType)
            return false;

        var nullableAttribute = parameter.GetCustomAttributes()
            .FirstOrDefault(a => a.GetType().FullName == "System.Runtime.CompilerServices.NullableAttribute");

        if (nullableAttribute == null) return false;
        var flagsField = nullableAttribute.GetType().GetField("NullableFlags");
        if (flagsField == null) return false;
        if (flagsField.GetValue(nullableAttribute) is not byte[] flags)
            return false;
        return flags.Length > 0 && flags[0] == 2;
    }
}
