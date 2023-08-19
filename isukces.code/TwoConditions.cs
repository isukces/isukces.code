namespace iSukces.Code;

public struct TwoConditions
{
    public TwoConditions(string condition, string negativeCondition)
    {
        Condition         = condition;
        NegativeCondition = negativeCondition;
    }

    public static TwoConditions FromEquals(string a, string b, bool useCs8IsNotNull = false)
    {
        if (b == "null")
        {
            return new TwoConditions(
                IsNull(a),
                IsNotNull(a, useCs8IsNotNull)
            );
        }

        if (a == "null")
        {
            return new TwoConditions(
                IsNull(b),
                IsNotNull(b, useCs8IsNotNull)
            );
        }

        return new TwoConditions($"{a} == {b}", $"{a} != {b}");
    }

    public static TwoConditions FromNegate(string boooleanExpression)
    {
        return new TwoConditions($"!{boooleanExpression}", boooleanExpression);
    }

    public static string IsNotNull(string expression, bool useCs8IsNotNull = false)
    {
        if (useCs8IsNotNull)
            return $"{expression} is not null";
        return $"!({expression} is null)";
    }

    public static string IsNull(string expression)
    {
        return $"{expression} is null";
    }

    #region Properties

    public string Condition         { get; }
    public string NegativeCondition { get; }

    #endregion
}
