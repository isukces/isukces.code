namespace isukces.code.AutoCode
{
    public struct CompareToExpressionData
    {
        public CompareToExpressionData(string fieldName, string compareExpression)
        {
            FieldName         = fieldName;
            CompareExpression = compareExpression;
        }

        public string FieldName         { get; }
        public string CompareExpression { get; }
    }
}