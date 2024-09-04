#nullable enable
using System;

namespace iSukces.Code;

public sealed class ConditionsPair
{
    public ConditionsPair(string condition, string? inversed = null)
    {
        Condition = condition;
        if (inversed is null)
            inversed = $"!({condition})";
        Inversed = inversed;
    }

    public static ConditionsPair FromInversed(string inversed)
    {
        return new ConditionsPair($"!({inversed})", inversed);
    }

    public bool IsAlwaysTrue => Condition == "true";

    public bool IsAlwaysFalse => Condition == "false";
    

    public static ConditionsPair FromIs(string variable, string type, string? variableName = null)
    {
        if (!string.IsNullOrEmpty(variableName))
            variableName = " " + variableName;
        var condition = $"{variable} is {type}{variableName}";
        if ((CsClass.DefaultCodeFormatting.Flags & CodeFormattingFeatures.IsNotNull) != 0)
            return new ConditionsPair(condition, $"{variable} is not {type}{variableName}");
        if (!string.IsNullOrEmpty(variableName))
            throw new NotSupportedException();
        return new ConditionsPair(condition, $"!({condition})");
    }

    public static ConditionsPair FromIsNull(string variable)
    {
        return new ConditionsPair($"{variable} is null");
    }

    public static implicit operator ConditionsPair(bool x)
    {
        return x
            ? new ConditionsPair("true", "false")
            : new ConditionsPair("false", "true");
    }

    public override string ToString()
    {
        return $"Condition={Condition}, Inversed={Inversed}";
    }

    public string Condition { get; }

    public string Inversed { get; }
}