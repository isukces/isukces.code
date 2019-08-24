namespace isukces.code.AutoCode
{
    public struct GetHashCodeExpressionData
    {
        public GetHashCodeExpressionData(CsExpression code, int? min = null, int? max = null)
        {
            Code = code;
            Min  = min;
            Max  = max;
        }

        public static implicit operator GetHashCodeExpressionData(CsExpression code) =>
            new GetHashCodeExpressionData(code);


        public CsExpression GetEffectiveCode1()
        {
            if (!Min.HasValue || Min.Value == 0)
                return Code;
            return Code - Min.Value;
        }

        public int GetGethashcodeMultiply(int defaultGethashcodeMultiply)
        {
            if (Max.HasValue && Min.HasValue)
                return Max.Value - Min.Value + 1;
            return defaultGethashcodeMultiply;
        }

        public override string ToString() => Code.Code;

        public CsExpression Code { get; }
        public int?         Min  { get; }
        public int?         Max  { get; }
    }
}