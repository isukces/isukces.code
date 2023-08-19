namespace iSukces.Code;

public struct TwoConditions
{
    public TwoConditions(string condition, string negativeCondition)
    {
        Condition         = condition;
        NegativeCondition = negativeCondition;
    }

    public static TwoConditions FromEquals(string a, string b)
    {
        if (b == "null")
            return new TwoConditions($"{a} is null", $"!({a} is null)");
        if (a == "null")
            return new TwoConditions($"{b} is null", $"!({b} is null)");
        return new TwoConditions($"{a} == {b}", $"{a} != {b}");
    }

    public static TwoConditions FromNegate(string boooleanExpression)
    {
        return new TwoConditions($"!{boooleanExpression}", boooleanExpression);
    }

    public string Condition         { get; }
    public string NegativeCondition { get; }
}
