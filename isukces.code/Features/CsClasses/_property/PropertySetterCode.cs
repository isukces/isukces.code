using System;

namespace iSukces.Code;

public sealed class PropertySetterCode
{
    public PropertySetterCode(string code, PropertyMetodKind kind)
    {
        Code = code;
        Kind = kind;
    }

    public static PropertySetterCode ExpressionBody(string value)
    {
        if (string.IsNullOrEmpty(value))
            throw new ArgumentException(nameof(value) + " is empty");
        return new PropertySetterCode(value, PropertyMetodKind.ExpressionBody);
    }

    public static explicit operator PropertySetterCode?(string? value)
    {
        if (string.IsNullOrEmpty(value))
            return null;
        return new PropertySetterCode(value, PropertyMetodKind.Body);
    }


    public static PropertySetterCode Value(string value)
    {
        if (string.IsNullOrEmpty(value))
            throw new ArgumentException(nameof(value) + " is empty");
        return new PropertySetterCode(value, PropertyMetodKind.Value);
    }

    public string            Code { get; }
    public PropertyMetodKind Kind { get; }
}
