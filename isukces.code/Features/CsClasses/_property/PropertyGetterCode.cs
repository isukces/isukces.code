using System;

namespace iSukces.Code;

public sealed class PropertyGetterCode
{
    public PropertyGetterCode(string code, PropertyMetodKind kind)
    {
        Code = code;
        Kind = kind;
    }

    public static PropertyGetterCode ExpressionBody(string value)
    {
        if (string.IsNullOrEmpty(value))
            throw new ArgumentException(nameof(value) + " is empty");
        return new PropertyGetterCode(value, PropertyMetodKind.ExpressionBody);
    }

    public static explicit operator PropertyGetterCode?(string? value)
    {
        if (string.IsNullOrEmpty(value))
            return null;
        return new PropertyGetterCode(value, PropertyMetodKind.Body);
    }


    public static PropertyGetterCode Value(string value)
    {
        if (string.IsNullOrEmpty(value))
            throw new ArgumentException(nameof(value) + " is empty");
        return new PropertyGetterCode(value, PropertyMetodKind.Value);
    }

    public string            Code { get; }
    public PropertyMetodKind Kind { get; set; }
}
