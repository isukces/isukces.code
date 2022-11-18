namespace iSukces.Code
{
    public sealed class ConditionsPair
    {
        public ConditionsPair(string condition, string inversed = null)
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
}