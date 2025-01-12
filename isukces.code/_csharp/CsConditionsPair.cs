#if OBSOLETE
namespace iSukces.Code;

public sealed class CsConditionsPair
{
    public CsConditionsPair(string condition, string? inversed = null)
    {
        Condition = condition;
        if (inversed is null)
            inversed = $"!({condition})";
        Inversed = inversed;
    }

    public static CsConditionsPair FromInversed(string inversed)
    {
        return new CsConditionsPair($"!({inversed})", inversed);
    }

    public bool IsAlwaysTrue
    {
        get
        {
            return Condition == "true";
        }
    }
    public bool IsAlwaysFalse
    {
        get
        {
            return Condition == "false";
        }
    }

    public static CsConditionsPair FromIsNull(string variable)
    {
        return new CsConditionsPair($"{variable} is null");
    }

    public static implicit operator CsConditionsPair(bool x)
    {
        return x
            ? new CsConditionsPair("true", "false")
            : new CsConditionsPair("false", "true");
    }

    public override string ToString()
    {
        return $"Condition={Condition}, Inversed={Inversed}";
    }

    public string Condition { get; }

    public string Inversed { get; }
}
#endif
