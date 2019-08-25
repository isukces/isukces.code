using isukces.code.AutoCode;

namespace isukces.code.FeatureImplementers
{
    public struct GetHashCodeExpressionData
    {
        public GetHashCodeExpressionData(CsExpression expression, int? min = null, int? max = null)
        {
            Expression = expression;
            Min        = min;
            Max        = max;
        }

        public static implicit operator GetHashCodeExpressionData(CsExpression code) =>
            new GetHashCodeExpressionData(code);

        public int GetGethashcodeMultiply(int defaultGethashcodeMultiply)
        {
            if (Max.HasValue && Min.HasValue)
                return Max.Value - Min.Value + 1;
            return defaultGethashcodeMultiply;
        }

        public override string ToString() => ExpressionWithOffset.Code;

        public CsExpression Expression { get; }

        public CsExpression ExpressionWithOffset
        {
            get { return Min is null ? Expression : Expression - Min.Value; }
        }

        public int?         Min        { get; }
        public int?         Max        { get; }

        public bool HasMinMax
        {
            get { return Min.HasValue && Max.HasValue; }
        }
    }
}